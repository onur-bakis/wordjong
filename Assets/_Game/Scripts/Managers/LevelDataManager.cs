using Scripts.Data.ValueObject;
using Scripts.Keys;
using UnityEngine;

namespace Scripts.Managers
{
    public class LevelDataManager
    {
        public static int currentLevelNumber;
        public static LevelData currentLevelData;
        public static LevelFinishParams levelFinishParams;

        public static void SetLevelNumber(int levelNumber)
        {
            currentLevelNumber = levelNumber;
        }
        public static LevelData GetLevelData()
        {
            return currentLevelData;
        } 
        public static LevelData GetLevelData(int i)
        {
            TextAsset textAsset = Resources.Load<TextAsset>($"LevelData/level_{(i%20)+1}");
            LevelData levelData = JsonUtility.FromJson<LevelData>(textAsset.text);
            currentLevelData = levelData;
            return levelData;
        }
        
        public static int GetLevelScore(int levelNumber)
        {
            return PlayerPrefs.GetInt("LHS" + levelNumber, -1);
        }
        public static void SetLevelHighScore(int levelNumber,int highScore)
        {
            PlayerPrefs.SetInt("LHS" + levelNumber, highScore);
            PlayerPrefs.Save();
        }

        public static void NewUnLock(int levelNumber)
        {
            PlayerPrefs.SetInt("NewUnLock",levelNumber);
            PlayerPrefs.Save();
        }

        public static int GetNewUnlock()
        {
            return PlayerPrefs.GetInt("NewUnLock",-1);
        }
    }
}