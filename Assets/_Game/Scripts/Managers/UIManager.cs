using Scripts.Context.Signals;
using Scripts.Controller.UI;
using Scripts.Enums;
using Scripts.Keys;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Scripts.Managers
{
    public class UIManager : View
    {
        [SerializeField] private UIPanelController _uiPanelController;
        [Inject] public CoreGameSignals CoreGameSignals { get; set; }
        [Inject] public CoreUISignals CoreUISignals { get; set; }
        
        protected override void Start()
        {
            OpenStartPanel();
        }
        public void OnTapToContinueButton()
        {
            CoreGameSignals.onTapToContinue?.Invoke();
            CoreGameSignals.onReset?.Invoke();
        }
        
        protected override void OnEnable()
        {
            base.OnEnable();
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CoreGameSignals.onPlayGameInitialize += OnPlayGameInitialize;
            CoreGameSignals.onLevelFinished += OnLevelFinished;
            CoreGameSignals.onReset += OnReset;
            CoreGameSignals.onTapToContinue += OnTapToContinue;
        }
  
        private void UnSubscribeEvents()
        {
            CoreGameSignals.onPlayGameInitialize -= OnPlayGameInitialize;
            CoreGameSignals.onLevelFinished -= OnLevelFinished;
            CoreGameSignals.onReset -= OnReset;
            CoreGameSignals.onTapToContinue -= OnTapToContinue;

        }
        protected override void OnDisable()
        {
            UnSubscribeEvents();
        }
        private void OpenStartPanel()
        {
            CoreUISignals.onOpenPanel?.Invoke(UIPanelTypes.Start);
        }
        public void OnSeeLevels()
        {
            CoreUISignals.onOpenPanel?.Invoke(UIPanelTypes.LevelSelection);
        }
        private void OnPlayGameInitialize(byte levelValue)
        {
            CoreUISignals.onOpenPanel?.Invoke(UIPanelTypes.Game);
            CoreGameSignals.onPlay?.Invoke();
        }
        
        private void OnLevelFinished(LevelFinishParams levelFinishParams)
        {
            CoreUISignals.onOpenPanel?.Invoke(UIPanelTypes.EndScreen);
        }

        private void OnTapToContinue()
        {
            CoreUISignals.onOpenPanel?.Invoke(UIPanelTypes.LevelSelection);
        }

        private void OnReset()
        {
        }
    }
}