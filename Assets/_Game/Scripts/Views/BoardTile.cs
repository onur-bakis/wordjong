using Scripts.Controller.GamePlay;
using DG.Tweening;
using strange.extensions.mediation.impl;
using TMPro;
using UnityEngine;
using TileData = Scripts.Data.ValueObject.TileData;

namespace Scripts.Views
{
    public class BoardTile : View
    {
        [SerializeField] private SpriteRenderer _tileBgSpriteRenderer;
        [SerializeField] private TextMeshPro _characterTextMeshProUGUI;

        private GameBoardController _gameBoardController;
        private Vector3 _position;
        public int _id;
        private int[] _children;
        
        public string character;
        public bool isLocked;
        public bool onBoard = true;
        public int parentCount;
        public bool removed = false;
        public bool inMove = false;

        public void Init(GameBoardController gameBoardController,TileData tileData)
        {
            _gameBoardController = gameBoardController;
            _id = tileData.id;
            _position = tileData.position;
            _children = tileData.children;
            character = tileData.character;

            ChangeView();
            MovePosition();
        }
        

        private void ChangeView()
        {
            _characterTextMeshProUGUI.text = character;
        }

        public void SetLockView(int parentChange = 0)
        {
            parentCount += parentChange;
            isLocked = parentCount != 0;
            _tileBgSpriteRenderer.gameObject.SetActive(parentCount!=0);
        }

        private void MovePosition()
        {
            transform.position = _position + Vector3.up * 50;
            transform.DOMove(_position,1.5f-((_position.y + 100)/30 - (_position.z - 99))/10f).SetEase(Ease.OutBack);
        }

        public void MoveToWord(Vector3 movePosition)
        {
            inMove = true;
            transform.DOMove(movePosition, 1f);
            onBoard = false;
            //Decrease parent count because execute action move parent to word placement
            _gameBoardController.ChangeParentCount(_children,-1);
        }

        public void MoveToTile(Vector3 mainPosition)
        {
            transform.DOMove(mainPosition, 1f).OnComplete(ChangeOnMove);
            onBoard = true;
            //Increase parent count because undo action has moved a parent back
            _gameBoardController.ChangeParentCount(_children,1);

        }

        private void ChangeOnMove()
        {
            inMove = false;
        }

        public void RemoveTile()
        {
            removed = true;
            transform.DOScale(Vector3.zero, 0.5f);
        }

        public void Shake()
        {
            transform.DOShakePosition(0.5f);
        }
    }
}