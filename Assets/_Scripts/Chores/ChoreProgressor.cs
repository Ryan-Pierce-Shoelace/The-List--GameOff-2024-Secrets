using System;
using UnityEngine;

namespace Horror.Chores
{
    public class ChoreProgressor : MonoBehaviour
    {
	    [SerializeField] private ChoreDataSO chore;

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
	    }
    }
}
