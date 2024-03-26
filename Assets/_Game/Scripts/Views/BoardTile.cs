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

        private float moveSpeed = 1200f;
        public Vector2 _cacheVector;
        public float duration;

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
            duration = 1f;
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
            
            float xDelay = (_position.x+50)/100f;
            float yDelay = (_position.y + 50f) / 100f;
            float zDelay = (_position.z + -70f) / 30f;
            float delayTotal = xDelay + yDelay + zDelay;
            
            transform.DOMove(_position,delayTotal/2f).SetEase(Ease.OutBack);
        }

        public void MoveToWord(Vector3 movePosition)
        {
            inMove = true;
            transform.DOKill();
            // _cacheVector = movePosition - transform.position;
            // float duration = _cacheVector.sqrMagnitude / moveSpeed;
            
            transform.DOMove(movePosition, duration);
            transform.DOScale(Vector3.one * 0.55f, duration);
            onBoard = false;
            //Decrease parent count because execute action move parent to word placement
            _gameBoardController.ChangeParentCount(children,-1);
        }

        public void MoveToTile(Vector3 mainPosition)
        {
            transform.DOKill();
            // _cacheVector = mainPosition - transform.position;
            // float duration = _cacheVector.sqrMagnitude / moveSpeed;
            
            transform.DOMove(mainPosition, duration).OnComplete(ChangeOnMove);
            transform.DOScale(Vector3.one, duration);
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

        public void Reset()
        {
            transform.localScale = Vector3.one;
            inMove = false;
            removed = false;
            onBoard = true;
        }
    }
}