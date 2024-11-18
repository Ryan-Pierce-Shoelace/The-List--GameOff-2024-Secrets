using System;
using UnityEngine;

namespace Horror.Chores
{
    public class ChoreProgressor : MonoBehaviour
    {
	    [SerializeField] private ChoreDataSO chore;
		[SerializeField] private ChoreRevealer revealOnComplete;
        public string GetChoreID() => chore.ID;
	    public ChoreState GetChoreState()
	    {
		    return !chore ? ChoreState.Hidden : ChoreManager.Instance.GetChoreState(chore);
	    }

	    public void ProgressChore()
	    {
		    if (!chore)
		    {
			    return;
		    }
		    ChoreEvents.AdvanceChore(chore.ID);

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
