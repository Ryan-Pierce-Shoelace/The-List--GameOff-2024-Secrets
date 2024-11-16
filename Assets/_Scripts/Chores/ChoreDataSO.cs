using System;
using System.Collections.Generic;
using UnityEngine;

namespace Horror.Chores
{
	[CreateAssetMenu(fileName = "New Chore", menuName = "Chores/Chore")]
	public class ChoreDataSO : ScriptableObject
	{
		public string ID;
		public string ChoreName;
		public List<string> RequiredChoreIds = new List<string>();

		public int CurrentCount { get; private set; }
		public int RequiredCount = 1;


		private void Awake()
		{
			Reset();
		}

		public void Reset()
		{
			CurrentCount = 0;
		}

		public void SetRequiredCount(int count)
		{
			RequiredCount = count;
		}


		public void Increment()
		{
			if(IsCompleted()) return;
			
			CurrentCount++;
			Debug.Log($"chore {ID}, has progressed to {CurrentCount}");
			if (IsCompleted())
			{
				ChoreEvents.CompleteChore(ID);
			}
		}


		public bool IsCompleted()
		{
			return CurrentCount >= RequiredCount;
		}
	}
}