using UnityEngine;

namespace Horror.Chores
{
    public class ChoreProgressor : MonoBehaviour
    {
	    [SerializeField] private ChoreDataSO chore;

	    public ChoreState GetChoreState()
	    {
		    return chore == null ? ChoreState.Hidden : ChoreManager.Instance.GetChoreState(chore);
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
