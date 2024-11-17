using UnityEngine;

namespace Horror.Chores
{
    public class ChoreProgressor : MonoBehaviour
    {
	    [SerializeField] private ChoreDataSO chore;

	    public ChoreState GetChoreState()
	    {
		   return ChoreManager.Instance.GetChoreState(chore);
	    }

	    public void ProgressChore()
	    {
		    ChoreEvents.AdvanceChore(chore.ID);
	    }
    }
}
