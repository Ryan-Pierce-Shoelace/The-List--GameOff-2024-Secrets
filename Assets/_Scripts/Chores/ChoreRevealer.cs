using Horror.Chores;
using System.Collections.Generic;
using UnityEngine;

namespace Horror
{
    public class ChoreRevealer : MonoBehaviour
    {
        [SerializeField] private List<ChoreDataSO> requiredChores;
        [SerializeField] private List<ChoreDataSO> choresToUnhide;

        //UNITY VOID EVENT
        public void TryRevealNewChores()
        {
            if(requiredChores.Count > 0)
            {
                bool allComplete = true;
                for(int i = 0; i < requiredChores.Count; i++)
                {
                    if (!requiredChores[i].IsCompleted())
                    {
                        allComplete = false;
                    }
                }

                if (!allComplete)
                    return;
            }


            for(int i = 0;i < choresToUnhide.Count; i++)
            {
                ChoreEvents.UnhideChore(choresToUnhide[i].ID);
            }
        }
    }
}
