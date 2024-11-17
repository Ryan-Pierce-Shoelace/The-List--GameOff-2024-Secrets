using System;
using System.Collections.Generic;
using UnityEngine;

namespace Horror.Chores
{
	[CreateAssetMenu(fileName = "New Chore", menuName = "Chores/Chore")]
	public class ChoreDataSO : ScriptableObject
	{
		public string ID;
		public string ChoreName = "Chore Name Not Found";
		[SerializeField] public List<ChoreDataSO> RequiredChores;

		[SerializeField] public List<ChoreDataSO> ChoresToUnhide;
		public bool StartsHidden = false;

		public int CurrentCount { get; private set; }
		public int RequiredCount = 1;

		public bool IsTutorialChore => isTutorialChore;
		[SerializeField] private bool isTutorialChore = false;


		private void Awake()
		{
			// RequiredChores = new List<ChoreDataSO>();
			// ChoresToUnhide = new List<ChoreDataSO>();
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
			if (IsCompleted()) return;

			CurrentCount++;
			if (IsCompleted())
			{
				ChoreEvents.CompleteChore(ID);
				if (ChoresToUnhide is not { Count: > 0 }) return;

				foreach (ChoreDataSO chore in ChoresToUnhide)
				{
					ChoreEvents.UnhideChore(chore.ID);
				}
			}
		}


		public bool IsCompleted()
		{
			return CurrentCount >= RequiredCount;
		}
	}
}