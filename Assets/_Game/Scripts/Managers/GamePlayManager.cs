using Scripts.Controller.GamePlay;
using Scripts.Data.ValueObject;
using Scripts.Signals;
using Scripts.Views;
using Scripts.Context.Signals;
using Scripts.Keys;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Scripts.Managers
{
    public class GamePlayManager:View
    {
        [SerializeField] private GameBoardController _gameBoardController;
        [Inject] public CoreGameSignals CoreGameSignals { get; set; }
        [Inject] public TapSignal TapSignal { get; set; }
        [Inject] public GameBoardSignals GameBoardSignals { get; set; }
        [Inject] public GameUISignals GameUISignals { get; set; }

        private GameBoardClick _gameBoardClick;
        private GameBoardActions _gameBoardActions;
        private GameBoardCheckWord _gameBoardCheckWord;
        private GameBoardScore _gameBoardScore;
        
        private int _currentLevelNumber;
        private LevelData _currentLevelData;
        public BoardTile[] _currentBoardTiles;

        private WordCheckParams _cacheWordCheckParams;        
        private LevelFinishParams _cacheLevelFinishParams;

        protected override void Awake()
        {
            base.Awake();
            _gameBoardClick = new GameBoardClick(this);
            _gameBoardActions = new GameBoardActions(this);
            _gameBoardCheckWord = new GameBoardCheckWord(this);
            _gameBoardScore = new GameBoardScore(this);
        }
        
        protected override void OnEnable()
        {
            base.OnEnable();
            CoreGameSignals.onPlay += OnPlayGame;
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();
            CoreGameSignals.onPlay -= OnPlayGame;
        }
        private void OnPlayGame()
        {
            _currentLevelNumber = LevelDataManager.currentLevelNumber;
            _currentLevelData = LevelDataManager.currentLevelData;
            
            _currentBoardTiles = _gameBoardController.SetBoard(_currentLevelNumber,_currentLevelData);
            _gameBoardClick.SetBoardValues(_currentBoardTiles);
            _gameBoardActions.Reset();

            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            TapSignal.AddListener(_gameBoardClick.OnInputTaken);
            GameUISignals.onUndo += _gameBoardActions.UndoTileMove;
            GameUISignals.onSubmit += SubmitWord;
        }

        private void UnSubscribeEvents()
        {
            TapSignal.RemoveListener(_gameBoardClick.OnInputTaken);
            GameUISignals.onUndo -= _gameBoardActions.UndoTileMove;
            GameUISignals.onSubmit -= SubmitWord;
        }

        public void SubmitWord()
        {
            string word = _gameBoardActions.word;
            //Word is added in found words
            _gameBoardCheckWord.Submit(word);
            //Actions are cleared
            _gameBoardActions.SubmitWord();
            //New total score is calculated
            int newScore = _gameBoardScore.AddScore(word);
            

            //Update UIs
            GameBoardSignals.onTopScoreChange.Invoke(newScore);
            GameBoardSignals.onNewWordAdded.Invoke(word);
            WordChange("");
            AvailUndo(false);
            
            bool remainAnyWords = _gameBoardCheckWord.CheckRemainWords(_gameBoardController.currentBoardTiles);
            
            if (!remainAnyWords)
            {
                //ResetScore TotalScore
                _gameBoardScore.Reset();
                //Check HighScore
                if (newScore > LevelDataManager.GetLevelScore(_currentLevelNumber))
                {
                    LevelDataManager.SetLevelHighScore(_currentLevelNumber,newScore);  
                    _cacheLevelFinishParams.highScore = true; 
                }
                else
                {
                    _cacheLevelFinishParams.highScore = false;
                }
                _cacheLevelFinishParams.score = newScore;
                
                UnSubscribeEvents();
                _gameBoardController.LevelFinished();
                int currentLock = LevelDataManager.GetNewUnlock();
                if (currentLock == _currentLevelNumber || currentLock == -1)
                {
                    LevelDataManager.NewUnLock(_currentLevelNumber+1);
                }

                LevelDataManager.levelFinishParams = _cacheLevelFinishParams;
                CoreGameSignals.onLevelFinished(_cacheLevelFinishParams);
            }
        }
        
        public void BoardTileClicked(BoardTile boardTile)
        {
            _gameBoardActions.MoveBoardTile(boardTile);
        }

        public void AvailUndo(bool available)
        {
            GameBoardSignals.onUndoAvailable(available);
        }

        public void WordChange(string currentWord)
        {
            _cacheWordCheckParams.score = _gameBoardScore.GetWordScore(currentWord);
            _cacheWordCheckParams.canSubmit = _gameBoardCheckWord.LookWordAvailable(currentWord);
            GameBoardSignals.onCurrentWordChange?.Invoke(_cacheWordCheckParams);
            
        }

    }
}