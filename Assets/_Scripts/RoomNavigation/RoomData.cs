using Cinemachine;
using Horror.Chores;
using RyanPierce.Events;
using System.Collections.Generic;
using UI.Thoughts;
using UnityEngine;
using UnityEngine.Serialization;

namespace Horror.RoomNavigation
{
    public class RoomData : MonoBehaviour
    {
        [System.Serializable]
        public class RoomEntryTrigger
        {
            public ChoreDataSO[] ChoresRequiredForActivation;
            [FormerlySerializedAs("activateEvent")] public VoidEvent ActivateEvent;
            [FormerlySerializedAs("dynamicThought")] public DynamicThoughtSO DynamicThought;
            [FormerlySerializedAs("choreProgressor")] public ChoreProgressor ChoreProgressor;
            [FormerlySerializedAs("triggerOneShot")] public bool TriggerOneShot = false;
        }

        public string RoomName;

        [SerializeField] private CinemachineVirtualCamera roomCamera;
       
        [SerializeField] private RoomEntryTrigger[] enterRoomTriggers;

        private List<RoomEntryTrigger> currentTriggers;

        private ChoreProgressor[] roomChores;

        private void Start()
        {
            SearchRoomChores();
            currentTriggers = new List<RoomEntryTrigger>(enterRoomTriggers);
        }

        private void SearchRoomChores()
        {
            roomChores = GetComponentsInChildren<ChoreProgressor>(false);
        }
        public void OnEnterRoom()
        {
            for (int i = 0;i < currentTriggers.Count;i++)
            {
                if (currentTriggers[i] == null)
                    continue;


                bool allChoresFinished = true;
                for (int j = 0; j < currentTriggers[i].ChoresRequiredForActivation.Length; j++)
                {
                    ChoreState requiredChoreState = ChoreManager.Instance.GetChoreState(currentTriggers[i].ChoresRequiredForActivation[j]);

                    if (requiredChoreState != ChoreState.Completed)
                        allChoresFinished = false;
                }

                if (!allChoresFinished)
                    continue;

                if (currentTriggers[i].ActivateEvent != null)
                {
                    currentTriggers[i].ActivateEvent?.Raise();
                }

                if (currentTriggers[i].ChoreProgressor != null)
                {
                    ChoreState progressorState = currentTriggers[i].ChoreProgressor.GetChoreState();
                    bool isChoreStateValid = progressorState == ChoreState.Available || progressorState == ChoreState.Completed;

                    if (isChoreStateValid)
                    {
                        currentTriggers[i].ChoreProgressor?.ProgressChore();
                    }
                }

                if (currentTriggers[i].DynamicThought != null)
                {
                    currentTriggers[i].DynamicThought.PlayThought();
                }

                SearchRoomChores();

                if (currentTriggers[i].TriggerOneShot)
                {
                    currentTriggers.RemoveAt(i);
                }
            }
        }

        public void GetRoomCompletion(out int numChoresComplete, out int numTotalChores)
        {
            HashSet<string> searchedChoreID = new HashSet<string>();
            numChoresComplete = 0;
            numTotalChores = 0;

            foreach (ChoreProgressor t in roomChores)
            {
                if (searchedChoreID.Contains(t.GetChoreID()))
                {
                    continue;
                }

                searchedChoreID.Add(t.GetChoreID());
                
                switch (t.GetChoreState())
                {
                    case ChoreState.Available:
                        numTotalChores++;
                        break;
                    case ChoreState.Completed:
                        numChoresComplete++;
                        numTotalChores++;
                        break;
                }
            }
        }
        public void ToggleRoomCamera(bool toggle)
        {
            roomCamera.gameObject.SetActive(toggle);
        }

    }
}
