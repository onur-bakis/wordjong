using System.Collections.Generic;
using Scripts.Managers;
using UnityEngine;

namespace Scripts.Controller.GamePlay
{
    public class GameBoardScore
    {
        public int score;
        private Dictionary<char, int> charScore;
        private GamePlayManager _gamePlayManager;

        private int totalScore;
        private int lastWordScore;
        
        public GameBoardScore(GamePlayManager gamePlayManager)
        {
            _gamePlayManager = gamePlayManager;
            
            #region CharValues
            charScore = new Dictionary<char, int>()
            {
                {'e', 1},
                {'a', 1},
                {'o', 1},
                {'n', 1},
                {'r', 1},
                {'t', 1},
                {'l', 1},
                {'s', 1},
                {'u', 1},
                {'i', 1},
                {'d', 2},
                {'g', 2},
                {'b', 3},
                {'c', 3},
                {'m', 3},
                {'p', 3},
                {'f', 4},
                {'h', 4},
                {'v', 4},
                {'w', 4},
                {'y', 4},
                {'k', 5},
                {'j', 8},
                {'x', 8},
                {'q', 10},
                {'z', 10},
            };
            #endregion
            
        }
        public int GetWordScore(string word)
        {
            //Look for better way to calculate
            //change chars or dict instead of lower the whole word
            word = word.ToLower();
            int wordLength = word.Length;
            int score = 0;
            foreach (char ch in word)
            {
                score += 10 * wordLength * charScore[ch];
            }
            
            return score;
        }

        public void AddScore(int wordScore)
        {
            totalScore += wordScore;
        }
        public int AddScore(string word)
        {
            totalScore += GetWordScore(word);
            return totalScore;
        }

        public int GetTotalScore()
        {
            return totalScore;
        }

        public void Reset()
        {
            totalScore = 0;
        }
        
        
    }
}