using System;
using System.Collections.Generic;
using Horror.Chores.HorrorEffect;
using UnityEngine;
using Utilities;

namespace Horror.Chores
{
	[CreateAssetMenu(fileName = "New Day Plan", menuName = "Chores/Day Plan")]
	public class DayPlan : ScriptableObject
	{
		public string DayName;
		public List<ChoreDataSO> Chores;
		public Dictionary<ChoreDataSO, HorrorEffectData> DailyHorrorEffects = new Dictionary<ChoreDataSO, HorrorEffectData>();

		[SerializeField] private UnitySerializedDictionary<ChoreDataSO, HorrorTextInfo> horrorChoreText = new UnitySerializedDictionary<ChoreDataSO, HorrorTextInfo>();


		public void ResetAllChores()
		{
			foreach (ChoreDataSO chore in Chores)
			{
				chore.Reset();
			}
		}

		private void OnEnable()
		{
			ResetAllChores();
			CreateHorrorEffects();
		}


		private void CreateHorrorEffects()
		{
			DailyHorrorEffects.Clear();

			foreach (KeyValuePair<ChoreDataSO, HorrorTextInfo> kvp in horrorChoreText)
			{
				HorrorEffectData scaryData = new HorrorEffectData(kvp.Value.ScaryText, kvp.Value.Duration, kvp.Value.LikelyHood);

				DailyHorrorEffects.Add(kvp.Key, scaryData);
			}
		}

		[Serializable]
		public struct HorrorTextInfo
		{
			public string ScaryText;
			public float Duration;
			public float LikelyHood;
		}
	}
}