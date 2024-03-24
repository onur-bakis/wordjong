using UnityEngine;

namespace Scripts.Models
{
	public class GameModel {
		
		private const string KeyHighestScore = "highest_score";
		
		public int Score {
			get
			{
				return PlayerPrefs.GetInt(KeyHighestScore, 0);
			}
			set
			{
				PlayerPrefs.SetInt(KeyHighestScore,value);
			}
		}
       
		public void Save()
		{
			PlayerPrefs.Save();
		}
	}
}
