using Scripts.Controller;
using Scripts.Data.ValueObject;
using Scripts.Signals;
using Scripts.Context.Signals;
using Scripts.Keys;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Scripts.Managers
{
    public class GameUIManager : View
    {
        [Inject] public GameBoardSignals GameBoardSignals { get; set; }
        [Inject] public CoreGameSignals CoreGameSignals { get; set; }
        
        [Inject] public GameUISignals GameUISignals { get; set; }

        [SerializeField] private GameUIController _gameUIController;
        
        private LevelData _currentLevelData;
        
        protected override void Awake()
        {
            base.Awake();
            _currentLevelData = new LevelData();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            CoreGameSignals.onPlay += OnGameStart;
            GameBoardSignals.onUndoAvailable += _gameUIController.OnChangeUndoVisual;
            GameBoardSignals.onCurrentWordChange += OnWordScoreChange;
            GameBoardSignals.onTopScoreChange += _gameUIController.OnTopScoreChange;
            GameBoardSignals.onNewWordAdded += _gameUIController.OnNewWordAdded;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            CoreGameSignals.onPlay -= OnGameStart;
            GameBoardSignals.onUndoAvailable -= _gameUIController.OnChangeUndoVisual;
            GameBoardSignals.onCurrentWordChange -= OnWordScoreChange;
            GameBoardSignals.onTopScoreChange -= _gameUIController.OnTopScoreChange;
            GameBoardSignals.onNewWordAdded -= _gameUIController.OnNewWordAdded;
        }

        private void OnGameStart()
        {
            _currentLevelData = LevelDataManager.currentLevelData;
            _gameUIController.SetStartValues(_currentLevelData.title);
        }

        private void OnWordScoreChange(WordCheckParams wordCheckParams)
        {
            _gameUIController.WordScoreChange(wordCheckParams.score, wordCheckParams.canSubmit);
        }

        public void OnSubmit()
        {
            GameUISignals.onSubmit?.Invoke();
        }
        public void OnUndo()
        {
            GameUISignals.onUndo?.Invoke();
        }
    }
}