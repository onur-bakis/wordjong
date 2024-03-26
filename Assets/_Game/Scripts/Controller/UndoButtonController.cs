using System;
using Scripts.Managers;
using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Game.Scripts.Controller
{
    public class UndoButtonController:View, IPointerDownHandler, IPointerUpHandler 
    {
        [SerializeField] private GameUIManager _gameUIManager;

        [Inject] public GameUIManager GameUIManager { get; set; }
        
        private float waitTime = 0.35f;
        private float timePressed;
        private bool isPressed;

        private void OnHold()
        {
            GameUIManager.OnUndo();
        }
        private void Update()
        {
            if (Time.time - timePressed > waitTime && isPressed)
            {
                timePressed = Time.time;
                OnHold();
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            isPressed = true;
            timePressed = Time.time;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            isPressed = false;
        }
    }
}