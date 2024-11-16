using UnityEngine;

namespace Horror.Chores
{
    public class ChoreProgressor : MonoBehaviour
    {
	    [SerializeField] private ChoreDataSO choreID;

	    public void ProgressChore()
	    {
		    choreID.Increment();
	    }
    }
}
