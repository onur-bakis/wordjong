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
        
        public GameBoardClick(GamePlayManager gamePlayManager)
        {
            _gamePlayManager = gamePlayManager;
        }

        public void SetBoardValues(BoardTile[] boardTiles)
        {
            _boardTiles = boardTiles;
        }

        public void OnInputTaken(InputDataParams inputDataParams)
        {
            //Cache Vector3
            Vector3 vector3 = Camera.main.ScreenToWorldPoint(
                new Vector3(inputDataParams.width, inputDataParams.height, 100f));
            foreach (var boardTile in _boardTiles)
            {
                if (boardTile.isLocked || !boardTile.onBoard || boardTile.removed || boardTile.inMove)
                {
                    continue;
                }


                if (Vector2.Distance(vector3, boardTile.transform.position)<6f)
                {
                    _gamePlayManager.BoardTileClicked(boardTile);
                    break;
                }
            }
            
        }

    }
}