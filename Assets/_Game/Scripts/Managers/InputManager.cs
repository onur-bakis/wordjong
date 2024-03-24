using Scripts.Context.Signals;
using Scripts.Keys;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Scripts.Managers
{
    public class InputManager : View
    {
        [Inject] 
        public TapSignal TapSignal { get; set; }

        private InputDataParams _inputDataParams;
        private float _clickInterval = 0.4f;
        private float _lastClickTime;
        protected override void Awake()
        {
            base.Start();
            _lastClickTime = int.MinValue;
        }

        public void Update()
        {
            if (Time.timeSinceLevelLoad - _lastClickTime < _clickInterval) return;
            
            if(Input.GetMouseButton(0))
            {
                _lastClickTime = Time.timeSinceLevelLoad;
                _inputDataParams.width = (int)Input.mousePosition.x;
                _inputDataParams.height = (int)Input.mousePosition.y;
                
                TapSignal.Dispatch(_inputDataParams);
            }
        }
    }
}