using System;
using UnityEngine;

namespace Horror.Chores
{
    public class ChoreProgressor : MonoBehaviour
    {
	    [SerializeField] private ChoreDataSO chore;
		[SerializeField] private ChoreRevealer revealOnComplete;
        public string GetChoreID() => chore != null ? chore.name : $"{transform.name} has a progressor but no chore assigned to it";
	    public ChoreState GetChoreState()
	    {
		    return !chore ? ChoreState.Hidden : ChoreManager.Instance.GetChoreState(chore);
	    }
		public float GetChoreProgress()
		{
			return (float)chore.CurrentCount / (float)chore.RequiredCount;
		}

	    public void ProgressChore()
	    {
		    if (!chore)
		    {
			    return;
		    }
		    ChoreEvents.AdvanceChore(chore.name);

			if (revealOnComplete == null)
			{
                return;
            }
            if (ChoreManager.Instance.GetChoreState(chore) != ChoreState.Completed)
			{
				return;
			}

			revealOnComplete.TryRevealNewChores();
	    }
    }
}
