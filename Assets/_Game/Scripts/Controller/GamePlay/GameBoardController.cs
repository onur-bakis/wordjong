using System.Collections.Generic;
using DG.Tweening;
using Scripts.Data.ValueObject;
using Scripts.Views;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Scripts.Controller.GamePlay
{
    public class GameBoardController : View
    {
        [SerializeField] private BoardTile _boardTilePrefab;
        [SerializeField] private Transform _boardHolder;
        [SerializeField] private Mesh[] _meshes;
        [SerializeField] private Transform[] _transformsTileBG;
        
        public BoardTile[] currentBoardTiles;
        public Dictionary<char, Mesh> meshData;
        
        //Keeps how many parent has to be unlock, to be able to get clicked
        private int[] _currentBoardParentCount;
        
        private int _currentLevelNumber;
        private LevelData _currentLevelData;

        private BoardTile _cacheTileObject;
        private TileData _cacheTileData;

        private List<BoardTile> simplePool = new List<BoardTile>();

        protected override void Awake()
        {
            base.Awake();
            meshData = new Dictionary<char, Mesh>();
            List<char> englishAlphabet = new List<char>()
            {
                'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm',
                'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'
            };

            for (int i = 0; i < englishAlphabet.Count; i++)
            {
                meshData.Add(englishAlphabet[i],_meshes[i]);
            }
            
        }

        public BoardTile[] SetBoard(int currentLevelNumber,LevelData currentLevelData)
        {
            _currentLevelNumber = currentLevelNumber;
            _currentLevelData = currentLevelData;

            int tileNumber = _currentLevelData.tiles.Length;
            currentBoardTiles = new BoardTile[tileNumber];
            _currentBoardParentCount = new int[tileNumber];
            
            
            for (int i = 0; i < tileNumber; i++)
            {
                _cacheTileData = _currentLevelData.tiles[i];
                _cacheTileObject = GetBoardTils();
                
                currentBoardTiles[i] = _cacheTileObject;
                currentBoardTiles[i].Init(this,_cacheTileData);

                int childCount = _cacheTileData.children.Length;
                for (int j = 0; j < childCount; j++)
                {
                    _currentBoardParentCount[_cacheTileData.children[j]]++;
                }
            }

            for (int i = 0; i < tileNumber; i++)
            {
                currentBoardTiles[i].parentCount = _currentBoardParentCount[i];
                currentBoardTiles[i].SetLockView();
            }

            return currentBoardTiles;
        }

        public void ChangeParentCount(int[] children, int i)
        {
            foreach (int childId in children)
            {
                currentBoardTiles[childId].SetLockView(i);
            }
        }

        public void LevelFinished()
        {
            int tileNumber = _currentLevelData.tiles.Length;
            for (int i = 0; i < tileNumber; i++)
            {
                ReturnBoardTile(currentBoardTiles[i]);
            }
        }

        public void SubmitAnimation()
        {
            foreach (var tileBG in _transformsTileBG)
            {
                tileBG.DOScale(Vector3.zero, 0.25f);
                tileBG.DOScale(Vector3.one*4.5f, 0.25f).SetDelay(0.3f);

            }
        }
        public void OpenTiles(bool tilesOpen)
        {
            foreach (var tileBG in _transformsTileBG)
            {
                tileBG.localScale = tilesOpen?Vector3.one*4.5f:Vector3.zero;
                tileBG.gameObject.SetActive(tilesOpen);
            }
        }

        private int activePoolTiles;
        private BoardTile _cacheBoardTile;
        public BoardTile GetBoardTils()
        {
            if (simplePool.Count == 0)
            {
                _cacheBoardTile = Instantiate(_boardTilePrefab, _boardHolder);
                return _cacheBoardTile;
            }

            _cacheBoardTile = simplePool[simplePool.Count-1];
            _cacheBoardTile.gameObject.SetActive(true);
            _cacheBoardTile.Reset();
            simplePool.Remove(_cacheBoardTile);
            return _cacheBoardTile;
        }

        public void ReturnBoardTile(BoardTile boardTile)
        {
            boardTile.gameObject.SetActive(false);
            simplePool.Add(boardTile);
        }
    }
}