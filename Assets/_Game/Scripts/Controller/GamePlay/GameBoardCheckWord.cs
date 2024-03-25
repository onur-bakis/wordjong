using System;
using System.Collections.Generic;
using System.Linq;
using Scripts.Managers;
using Scripts.Views;
using UnityEngine;

namespace Scripts.Controller.GamePlay
{
    public class GameBoardCheckWord
    {
        public HashSet<string> _engWordList = new HashSet<string>();
        private GamePlayManager _gamePlayManager;
        private List<string> _foundWords;
        public GameBoardCheckWord(GamePlayManager gamePlayManager)
        {
            _gamePlayManager = gamePlayManager;
            
            TextAsset textAsset = Resources.Load<TextAsset>("Data/en");
            var listArray = textAsset.text.Split('\n');
            List<string> cache = listArray.ToList();
            cache.Remove("");
            _engWordList = new HashSet<string>(cache);
            _foundWords = new List<string>();

            // List<string> listtt = GeneratePermutations(new char[] { 'a', 's', 'd', 'f', 'e', 'u', 'i' }, 7);
            // foreach (var VARIABLE in listtt)
            // {
            //     Debug.Log(VARIABLE);
            // }
        }

        public void Reset()
        {
            _foundWords.Clear();
        }
        
        public void Submit(string word)
        {
            _foundWords.Add(word);
        }

        public bool LookWordAvailable(string word)
        {
            if (_foundWords.Contains(word))
            {
                return false;
            }

            return _engWordList.Contains(word.ToLower());
        }
        
        public bool CheckRemainWords(BoardTile[] boardTiles)
        {
            //Better Alg
            //Add locked depth tiles
            List<char> charSet = new List<char>();
            
            for (int i = 0; i < boardTiles.Length; i++)
            {
                if (!boardTiles[i].isLocked && !boardTiles[i].removed)
                {
                    charSet.Add(boardTiles[i].character);
                }
            }
            
            
            bool remaining = false;
             //Check open tiles only
             remaining = CheckPermutationsOpen(charSet.ToArray(),7);
            
            
             //Check closed tiles for 3 length words only(for optimization)
             if (!remaining)
             {
                 remaining = CheckPermutationsLocked(boardTiles, 3);
             }
            
            //Check closed tiles for any solution (7 length words)
            if (!remaining)
            {
                remaining = CheckPermutationsLocked(boardTiles, 7);
            }
            
            return remaining;
        }


        #region PermutationsLocked

        private Dictionary<int, BoardTile> _cleanedBoardTiles;
        private List<Char> _remainingChars;
        private string _remainingWord;
        private bool lockedWord;
        private bool CheckPermutationsLocked(BoardTile[] boardTiles, int maxLength)
        {
            _cleanedBoardTiles = new Dictionary<int, BoardTile>();
            _remainingChars = new List<char>();
            lockedWord = false;
            CleanBoardTiles(boardTiles);
            Debug.Log("CheckPermutationsLockedBefore");
            GeneratePermutationsLocked(_remainingChars.ToArray(),maxLength);
            Debug.Log("CheckPermutationsLocked"+j);
            return lockedWord;
        }

        private void CheckString(string word)
        {
            //Debug.Log("-----------------------");
            //Debug.Log("CheckString"+word);
            if (_engWordList.Contains(word.ToLower()))
            {
                i = 0;
                _remainingWord = "";
                //Debug.Log("EngWord"+word);
                PlayBoard(word,0,_cleanedBoardTiles);
                //Debug.Log("PlayWord "+i+" Try"+_remainingWord);
                if (_remainingWord != "")
                {
                    lockedWord = true;
                }
            }
        }

        private int i = 0;
        private void PlayBoard(string goalWord,int charIndex,Dictionary<int,BoardTile> cleanedBoardTiles)
        {
            i++;
            if (i > 99999)
            {
                Debug.LogError("I"+9999);
                return;
            }

            foreach (var idTile in cleanedBoardTiles)
            {
                if (charIndex == goalWord.Length)
                {
                    _remainingWord = goalWord;
                    Debug.Log("goalWord"+_remainingWord);
                    return;
                }

                bool unlocked = !idTile.Value.isLocked;
                bool charMatch = idTile.Value.character == goalWord[charIndex];

                if (unlocked && charMatch)
                {
                    idTile.Value.removed = true;
                    for (int i = 0; i < idTile.Value.children.Length; i++)
                    {
                        cleanedBoardTiles[idTile.Value.children[i]].SetLockView(-1);
                    }

                    PlayBoard(goalWord, charIndex+1, cleanedBoardTiles);
                    
                    idTile.Value.removed = false;
                    for (int i = 0; i < idTile.Value.children.Length; i++)
                    {
                        cleanedBoardTiles[idTile.Value.children[i]].SetLockView(+1);;
                    }
                    
                }
            }
        }

        private void CleanBoardTiles(BoardTile[] boardTiles)
        {
            foreach (var bTile in boardTiles)
            {
                if (!bTile.removed)
                {
                    _remainingChars.Add(bTile.character);
                    _cleanedBoardTiles.Add(bTile._id,bTile);
                }
            }
        }
        
        private int j = 0;
        public void GeneratePermutationsLocked(char[] charList, int maxLength)
        {
            j = 0;
            GeneratePermutationsHelperLocked(charList, new bool[charList.Length], "", 0, maxLength);
        }

        public void GeneratePermutationsHelperLocked(char[] charList, bool[] used, string current, int length, int maxLength)
        {
            j++;
            if (lockedWord)
            {
                return;
            }
            if (j > 99999)
            {
                Debug.LogError("J"+9999);
                return;
            }
            if (length > maxLength)
            {
                return;
            }

            if (length > 0)
            {
                CheckString(current);
                Debug.Log(current);
            }

            for (int i = 0; i < charList.Length; i++)
            {
                if (!used[i])
                {
                    used[i] = true;
                    GeneratePermutationsHelperLocked(charList, used, current + charList[i], length + 1, maxLength);
                    used[i] = false;
                }
            }
        }
        #endregion
        
        #region PermutationsOpen

        private bool breakRecursive = false;
        public bool CheckPermutationsOpen(char[] charList,int maxLength)
        {
            breakRecursive = false;
            GeneratePermutationsOpen(charList,maxLength);
            if (breakRecursive)
            {
                return true;
            }
            return false;
        }
        
        public void GeneratePermutationsOpen(char[] charList, int maxLength)
        {
            GeneratePermutationsHelperOpen(charList, new bool[charList.Length], "", 0, maxLength);
        }

        public void GeneratePermutationsHelperOpen(char[] charList, bool[] used, string current, int length, int maxLength)
        {
            if(breakRecursive)
                return;
            
            if (length > 0)
            {
                if (_engWordList.Contains(current))
                {
                    breakRecursive = true;
                }
            }

            for (int i = 0; i < charList.Length; i++)
            {
                if (!used[i])
                {
                    used[i] = true;
                    GeneratePermutationsHelperOpen(charList, used, current + charList[i], length + 1, maxLength);
                    used[i] = false;
                }
            }
        }
        #endregion
    }
}