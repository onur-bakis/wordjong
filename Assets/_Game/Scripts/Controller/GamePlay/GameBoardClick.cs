using Scripts.Managers;
using Scripts.Views;
using Scripts.Keys;
using UnityEngine;

namespace Scripts.Controller.GamePlay
{
    public class GameBoardClick
    {

        private GamePlayManager _gamePlayManager;
        private BoardTile[] _boardTiles;
        
        private Vector3 _cacheCameraRay;
        private Vector3 _cacheCameraRayResults;
        private float cameraDistance = 50f;
        
        public GameBoardClick(GamePlayManager gamePlayManager)
        {
            _gamePlayManager = gamePlayManager;
            _cacheCameraRay = Vector3.zero;
            _cacheCameraRay.z = cameraDistance;
        }

        public void SetBoardValues(BoardTile[] boardTiles)
        {
            _boardTiles = boardTiles;
        }

        public void OnInputTaken(InputDataParams inputDataParams)
        {
            _cacheCameraRay.x = inputDataParams.width;
            _cacheCameraRay.y = inputDataParams.height;
            _cacheCameraRayResults = Camera.main.ScreenToWorldPoint(_cacheCameraRay);
            
            foreach (var boardTile in _boardTiles)
            {
                if (boardTile.isLocked || !boardTile.onBoard || boardTile.removed || boardTile.inMove)
                {
                    continue;
                }

                if (Vector2.Distance(_cacheCameraRayResults, boardTile.transform.position)<3.5f)
                {
                    _gamePlayManager.BoardTileClicked(boardTile);
                    break;
                }
            }
            
        }

    }
}