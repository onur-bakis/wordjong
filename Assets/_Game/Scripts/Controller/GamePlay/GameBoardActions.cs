using System.Collections.Generic;
using Scripts.Commands.Actions;
using Scripts.Commands.Recorders;
using Scripts.Managers;
using Scripts.Views;
using UnityEngine;

namespace Scripts.Controller.GamePlay
{
    public class GameBoardActions
    {
        private ActionRecorder _actionRecorder;
        private int wordCount;
        private Vector3 startPosition;
        private Vector3 startPositionIncrease;
        private bool undoActive;
        private GamePlayManager _gamePlayManager;

        public string word="";
        public Stack<BoardTile> currentWordTiles;

        private Vector3 _cachePosition;

        public GameBoardActions(GamePlayManager gamePlayManager)
        {
            _actionRecorder = new ActionRecorder();
            currentWordTiles = new Stack<BoardTile>();
            _gamePlayManager = gamePlayManager;
            
            startPosition = new Vector3(10.2f,6f,99f);
            startPositionIncrease = Vector3.left * -4.6f;
        }

        public void Reset()
        {
            foreach (var boardTile in currentWordTiles)
            {
                boardTile.RemoveTile();
            }
            
            word = "";
            currentWordTiles = new Stack<BoardTile>();
            _actionRecorder = new ActionRecorder();
            wordCount = 0;
        }

        public void MoveBoardTile( BoardTile boardTile)
        {
            if (wordCount >= 7)
            {
                return;
            }
            _cachePosition = startPosition + startPositionIncrease * wordCount;
            BoardTileMoveAction boardTileMoveAction = new BoardTileMoveAction(ref _cachePosition,ref boardTile);
            _actionRecorder.Record(boardTileMoveAction);
            wordCount++;
            word += boardTile.character;
            currentWordTiles.Push(boardTile);
            
            _gamePlayManager.AvailUndo(wordCount!=0);
            _gamePlayManager.WordChange(word);
        }
        
        public void UndoTileMove()
        {
            if (wordCount == 0)
            {
                return;
            }
            _actionRecorder.Rewind();
            wordCount--;
            word = word.Remove(word.Length - 1, 1);
            currentWordTiles.Pop();
            
            _gamePlayManager.AvailUndo(wordCount!=0);
            _gamePlayManager.WordChange(word);
        }

        public void SubmitWord()
        {
            Reset();
            // foreach (var boardTile in currentWordTiles)
            // {
            //     boardTile.Shake();
            // }

        }
    }
}