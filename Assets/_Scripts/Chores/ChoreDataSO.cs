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
		public string Description;
		
		public int RequiredCount = 1;  
        
		private ChoreProgress progress;
		public ChoreProgress Progress => progress ??= new ChoreProgress { RequiredCount = RequiredCount };

		public void Increment()
		{
			progress.CurrentCount++;
			if (progress.IsCompleted)
			{
				ChoreEvents.CompleteChore(ID);
			}
			else
			{
				ChoreEvents.AdvanceChore(ID);
			}
		}
		
		public void Reset()
		{
			progress = new ChoreProgress { RequiredCount = RequiredCount };
		}
		
	}
	[Serializable]
	public class ChoreProgress
	{
		public int CurrentCount;
		public int RequiredCount = 1;
		public bool IsCompleted => CurrentCount >= RequiredCount;
	}

}