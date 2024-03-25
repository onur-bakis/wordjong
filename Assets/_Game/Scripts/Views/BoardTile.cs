using Scripts.Controller.GamePlay;
using DG.Tweening;
using strange.extensions.mediation.impl;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using TileData = Scripts.Data.ValueObject.TileData;

namespace Scripts.Views
{
    public class BoardTile : View
    {
        [SerializeField] private SpriteRenderer _tileBgSpriteRenderer;
        [SerializeField] private TextMeshPro _characterTextMeshProUGUI;
        [SerializeField] private MeshFilter _meshFilter;

        private GameBoardController _gameBoardController;
        private Vector3 _position;
        public int _id;
        public int[] children;
        public Color _openColor;
        public Color _closedColor;
        
        public char character;
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
            children = tileData.children;
            character = tileData.character.ToLower()[0];

            _openColor = new Color(1f,1f,1f,0f);
            _closedColor = new Color(1f,1f,1f,0.8f);

            ChangeView();
            MovePosition();
        }
        

        private void ChangeView()
        {
            //_characterTextMeshProUGUI.text = character.ToString();
            _meshFilter.mesh = _gameBoardController.meshData[character];
        }

        public void SetLockView(int parentChange = 0)
        {
            parentCount += parentChange;
            isLocked = parentCount != 0;
            //_tileBgSpriteRenderer.gameObject.SetActive(parentCount!=0);
            _tileBgSpriteRenderer.DOKill();
            //_tileBgSpriteRenderer.DOColor(isLocked?_closedColor:_openColor,1f);
            _tileBgSpriteRenderer.DOFade(isLocked ? 0.9f : 0f, 1f);
        }

        private void MovePosition()
        {
            transform.position = _position + Vector3.up * 50;
            transform.DOMove(_position,1.5f-((_position.y + 100)/30 - (_position.z - 99))/10f).SetEase(Ease.OutBack);
        }

        public void MoveToWord(Vector3 movePosition)
        {
            inMove = true;
            transform.DOKill();
            transform.DOMove(movePosition, 1f);
            transform.DOScale(Vector3.one * 0.6f, 1f);
            onBoard = false;
            //Decrease parent count because execute action move parent to word placement
            _gameBoardController.ChangeParentCount(children,-1);
        }

        public void MoveToTile(Vector3 mainPosition)
        {
            transform.DOKill();
            transform.DOMove(mainPosition, 1f).OnComplete(ChangeOnMove);
            transform.DOScale(Vector3.one, 1f);
            onBoard = true;
            //Increase parent count because undo action has moved a parent back
            _gameBoardController.ChangeParentCount(children,1);

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