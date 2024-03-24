using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Views.Common
{
    [RequireComponent(typeof(Button))]
    public class BaseButton : View
    {
        
        protected Button Button;

        protected override void Start()
        {
            Button = GetComponent<Button>();
            if (Button != null)
            {
                Button.onClick.AddListener(OnButtonClick);
            }
            base.Start();
        }
        
        public virtual void OnButtonClick()
        {
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            if (Button != null)
            {
                Button.onClick.RemoveListener(OnButtonClick);
            }
        }
    }
}