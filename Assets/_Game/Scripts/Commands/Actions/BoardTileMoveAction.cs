using Scripts.Commands.Base;
using Scripts.Views;
using UnityEngine;

namespace Scripts.Commands.Actions
{
    public class BoardTileMoveAction : ActionBase
    {
        private Vector3 _mainPosition;
        private Vector3 _movePosition;
        private BoardTile _boardTile;
        
        
        public BoardTileMoveAction(ref Vector3 movePosition,ref BoardTile boardTile)
        {
            _boardTile = boardTile;
            _movePosition = movePosition;
            _mainPosition = boardTile.transform.position;
        }
        public override void Execute()
        {
            _boardTile.MoveToWord(_movePosition);
        }

        public override void Undo()
        {
            _boardTile.MoveToTile(_mainPosition);
        }
    }
}