using System.Collections.Generic;
using System.Linq;
using Scripts.Managers;
using Scripts.Views;
using UnityEngine;

namespace Scripts.Controller.GamePlay
{
    public class GameBoardCheckWord
    {
        public List<string> _engWordList = new List<string>();
        private GamePlayManager _gamePlayManager;
        private List<string> _foundWords;
        public GameBoardCheckWord(GamePlayManager gamePlayManager)
        {
            _gamePlayManager = gamePlayManager;
            
            TextAsset textAsset = Resources.Load<TextAsset>("Data/en");
            var listArray = textAsset.text.Split('\n');
            _engWordList = listArray.ToList();
            _engWordList.Remove("");
            _foundWords = new List<string>();
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
            bool canSubmit = true;
            
            if (_foundWords.Contains(word))
            {
                canSubmit = false;
            }
            
            if (_engWordList.Contains(word.ToLower()))
            {
                canSubmit = true;
            }
            else
            {
                canSubmit = false;
            }

            return canSubmit;
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
                    charSet.Add(boardTiles[i].character.ToLower()[0]);
                }
            }

            bool remaining = CheckPermutations(charSet.ToArray(),7);

            return remaining;
        }

        
        #region Permutations
        public bool CheckPermutationsWithLock(string startString,char[] charList,int maxLength)
        {
            List<string> permutations = GeneratePermutations(charList,maxLength);
        
            permutations.Remove("");
            foreach (string permutation in permutations)
            {
                if (_engWordList.Contains(startString+permutation))
                {
                    return true;
                }
            }

            return false;
        }
        
        public bool CheckPermutations(char[] charList,int maxLength)
        {
            List<string> permutations = GeneratePermutations(charList,maxLength);
        
            permutations.Remove("");
            foreach (string permutation in permutations)
            {
                if (_engWordList.Contains(permutation))
                {
                    return true;
                }
            }
            return false;
        }
        
        static List<string> GeneratePermutations(char[] charList, int maxLength)
        {
            List<string> permutations = new List<string>();
            GeneratePermutationsHelper(charList, new bool[charList.Length], "", 0, maxLength, permutations);
            return permutations;
        }

        static void GeneratePermutationsHelper(char[] charList, bool[] used, string current, int length, int maxLength, List<string> permutations)
        {
            if (length > maxLength)
            {
                return;
            }

            if (length > 0)
            {
                permutations.Add(current);
            }

            for (int i = 0; i < charList.Length; i++)
            {
                if (!used[i])
                {
                    used[i] = true;
                    GeneratePermutationsHelper(charList, used, current + charList[i], length + 1, maxLength, permutations);
                    used[i] = false;
                }
            }
        }
        #endregion
    }
}