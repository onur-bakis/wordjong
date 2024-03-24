using Scripts.Data.ValueObject;
using Scripts.Managers;
using Scripts.Views;
using Scripts.Context.Signals;
using Scripts.Views.Common;
using UnityEngine;

namespace Scripts.Controller.UI
{
    public class LevelSelectionPanel : PanelBase
    {
        [SerializeField] private LevelSelectionButton _levelSelectButton;
        [SerializeField] private Transform LevelSelectionButtonHolder;
        
        private LevelData[] _levelData;
        private LevelSelectionButton[] _levelSelectionButtons;

        [Inject] public UIManager UIManager { get; set; }
        [Inject] public CoreGameSignals CoreGameSignals { get; set; }

        public override void Init()
        {
            base.Init();
            _levelData = new LevelData[20];
            _levelSelectionButtons = new LevelSelectionButton[20];
            
            for (int i = 0; i < 20; i++)
            {
                _levelData[i] = LevelDataManager.GetLevelData(i);
                _levelSelectionButtons[i] = Instantiate(_levelSelectButton,
                    LevelSelectionButtonHolder);
                
                int highScore = LevelDataManager.GetLevelScore(i);
                int lastUnlock = LevelDataManager.GetNewUnlock();
                
                _levelSelectionButtons[i].Init(this,i,_levelData[i].title,highScore,lastUnlock);
            }
        }

        public override void Show()
        {
            base.Show();
            for (int i = 0; i < 20; i++)
            {
                _levelData[i] = LevelDataManager.GetLevelData(i);
                int highScore = LevelDataManager.GetLevelScore(i);
                int lastUnlock = LevelDataManager.GetNewUnlock();
                
                _levelSelectionButtons[i].Init(this,i,_levelData[i].title,highScore,lastUnlock);
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