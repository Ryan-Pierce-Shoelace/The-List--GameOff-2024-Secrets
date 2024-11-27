using Horror.Chores;
using RyanPierce.Events;
using System.Collections.Generic;
using UnityEngine;

namespace Horror
{
    public class ChoreRevealer : MonoBehaviour
    {
        [SerializeField] private List<ChoreDataSO> requiredChores;
        [SerializeField] private List<ChoreDataSO> choresToUnhide;

        [SerializeField] private VoidEvent revealEvent;

        private bool revealed;
        //UNITY VOID EVENT
        public void TryRevealNewChores()
        {
            if(revealed) return;

            if(requiredChores.Count > 0)
            {
                bool allComplete = true;
                foreach (ChoreDataSO t in requiredChores)
                {
                    if (!t.IsCompleted())
                    {
                        allComplete = false;
                    }
                }

                if (!allComplete)
                    return;
            }


            foreach (ChoreDataSO t in choresToUnhide)
            {
                ChoreEvents.UnhideChore(t.name);
            }
            if(revealEvent != null)
            {
                revealEvent?.Raise();
            }

            revealed = true;
        }
    }
}
