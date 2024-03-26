using Scripts.Data.ValueObject;
using Scripts.Managers;
using Scripts.Views;
using Scripts.Context.Signals;
using Scripts.Views.Common;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Scripts.Controller.UI
{
    public class LevelSelectionPanel : PanelBase
    {
        [SerializeField] private LevelSelectionButton _levelSelectButtonPrefab;
        [SerializeField] private RectTransform LevelSelectionButtonHolder;
        [SerializeField] private ScrollRect _scrollRect;
        
        private LevelData[] _levelData;
        private LevelSelectionButton[] _levelSelectionButtons;

        [Inject] public UIManager UIManager { get; set; }
        [Inject] public CoreGameSignals CoreGameSignals { get; set; }

        private LevelSelectionButton _currentUnLock;
        private bool _showAnim;
        private Rect _rect;
        private int levelNumber = 20;
        public override void Init()
        {
            base.Init();
            _levelData = new LevelData[levelNumber];
            _levelSelectionButtons = new LevelSelectionButton[levelNumber];
            
            for (int i = 0; i < 20; i++)
            {
                _levelData[i] = LevelDataManager.GetLevelData(i);
                _levelSelectionButtons[i] = Instantiate(_levelSelectButtonPrefab,
                    LevelSelectionButtonHolder);
                
                int highScore = LevelDataManager.GetLevelScore(i);
                int lastUnlock = LevelDataManager.GetNewUnlock();
                
                _levelSelectionButtons[i].Init(this,i,_levelData[i].title,highScore,lastUnlock,false);
                if (i == lastUnlock)
                {
                    _currentUnLock = _levelSelectionButtons[i];
                }
            }
        }

        public override void Show()
        {
            int lastUnlock = LevelDataManager.GetNewUnlock();
            
            base.Show();
            for (int i = 0; i < levelNumber; i++)
            {
                _levelData[i] = LevelDataManager.GetLevelData(i);
                int highScore = LevelDataManager.GetLevelScore(i);
                bool animInfo = false;
                
                if (i == lastUnlock && _currentUnLock != _levelSelectionButtons[i])
                {
                    animInfo = true;
                    _currentUnLock = _levelSelectionButtons[i];
                    _showAnim = true;
                }
                
                _levelSelectionButtons[i].Init(this,i,_levelData[i].title,highScore,lastUnlock,animInfo);
                
            }
            
            if (lastUnlock != -1)
            {
                _scrollRect.verticalScrollbar.value = 1 - (lastUnlock - 1f) / (levelNumber-3f);
            }

        }

        public override void OnShowComplete()
        {
            base.OnShowComplete();
            if (_showAnim && _currentUnLock != null)
            {
                _currentUnLock.PlayAnim();
                _showAnim = false;
            }
        }
        

        public void GoLevel(int id)
        {
            LevelDataManager.SetLevelNumber(id);
            LevelDataManager.GetLevelData(id);
            CoreGameSignals.onPlayGameInitialize?.Invoke((byte)id);
        }
    }
}