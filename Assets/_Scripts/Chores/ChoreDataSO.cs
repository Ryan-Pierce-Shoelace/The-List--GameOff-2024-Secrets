using RyanPierce.Events;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Horror.Chores
{
	[CreateAssetMenu(fileName = "New Chore", menuName = "Chores/Chore")]
	public class ChoreDataSO : ScriptableObject
	{
		public string ChoreName = "Chore Name Not Found";
		[SerializeField] private List<ChoreDataSO> requiredChores;
		[SerializeField] private List<ChoreDataSO> choresToUnhide;
		[SerializeField] private VoidEvent completeEvent;
		public List<ChoreDataSO> RequiredChores => requiredChores;
		public List<ChoreDataSO> ChoresToUnhide => choresToUnhide;
		
		
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
				ChoreEvents.CompleteChore(this.name);
				
				if(completeEvent != null)
				{
					completeEvent?.Raise();
				}

				if (ChoresToUnhide is not { Count: > 0 }) return;

				foreach (ChoreDataSO chore in ChoresToUnhide)
				{
					ChoreEvents.UnhideChore(chore.name);
				}
			}
		}


		public bool IsCompleted()
		{
			return CurrentCount >= RequiredCount;
		}
	}
}