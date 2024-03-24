using System;
using Scripts.Managers;
using Scripts.Enums;
using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Handler
{
    public class GameUIEventSubscriber:View
    {
        [SerializeField] private GameUIEventSubscribtionTypes type;
        [SerializeField] private Button button;

        [Inject] public GameUIManager GameUIManager { get; set; }
        

        public void ButtonClick()
        {
            switch (type)
            {
                case GameUIEventSubscribtionTypes.OnSubmit:
                {
                    GameUIManager.OnSubmit();
                    break;
                }
                case GameUIEventSubscribtionTypes.OnUndo:
                {
                    GameUIManager.OnUndo();
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        protected override void OnEnable()
        {
            base.OnEnable();
            SubscribeEvents();
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            UnSubscribeEvents();
        }
        private void SubscribeEvents()
        {
            button.onClick.AddListener(ButtonClick);
        }

        private void UnSubscribeEvents()
        {
            button.onClick.RemoveListener(ButtonClick);
        }
    }
}