using UnityEngine;

namespace Horror.Chores
{
    public class ChoreProgressor : MonoBehaviour
    {
	    [SerializeField] private ChoreDataSO chore;

	    public void ProgressChore()
	    {
		    ChoreEvents.AdvanceChore(chore.ID);
	    }
    }
}
