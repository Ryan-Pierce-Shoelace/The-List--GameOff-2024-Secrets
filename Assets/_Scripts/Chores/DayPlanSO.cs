using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Horror.Chores
{
	[CreateAssetMenu(fileName = "New Day Plan", menuName = "Chores/Day Plan")]
	public class DayPlan : ScriptableObject
	{
		public string DayName;
		public List<ChoreDataSO> Chores;
		
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
		}

	}
}