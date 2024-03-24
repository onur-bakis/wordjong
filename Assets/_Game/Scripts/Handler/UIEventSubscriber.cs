using System;
using Scripts.Enums;
using Scripts.Managers;
using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Handler
{
    public class UIEventSubscriber : View
    {

        [SerializeField] private UIEventSubscriptionTypes type;
        [SerializeField] private Button button;

        [Inject] public UIManager UIManager { get; set; }
        

        public void ButtonClick()
        {
            switch (type)
            {
                case UIEventSubscriptionTypes.OnSeeLevels:
                {
                    UIManager.OnSeeLevels();
                    break;
                }
                case UIEventSubscriptionTypes.OnTapToContinue:
                {
                    UIManager.OnTapToContinueButton();
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
        private void SubscribeEvents()
        {
            button.onClick.AddListener(ButtonClick);
            
        }

        private void UnSubscribeEvents()
        {
            button.onClick.RemoveListener(ButtonClick);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            UnSubscribeEvents();
        }
    }

    
}