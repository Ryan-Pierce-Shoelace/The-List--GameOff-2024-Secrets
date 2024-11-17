using Cinemachine;
using Horror.Chores;
using System.Collections.Generic;
using UI.Thoughts;
using UnityEngine;

namespace Horror.RoomNavigation
{
    public class RoomData : MonoBehaviour
    {
        [System.Serializable]
        public class RoomEntryTrigger
        {
            public ChoreDataSO ChoreRequiredForActivation;
            public DynamicThoughtSO dynamicThought;
            public ChoreProgressor choreProgressor;
        }

        public string RoomName;

        [SerializeField] private CinemachineVirtualCamera roomCamera;
       
        [SerializeField] private RoomEntryTrigger[] enterRoomTriggers;

        private List<RoomEntryTrigger> currentTriggers;

        private ChoreProgressor[] roomChores;

        private void Start()
        {
            roomChores = GetComponentsInChildren<ChoreProgressor>();
            currentTriggers = new List<RoomEntryTrigger>(enterRoomTriggers);
        }
        public void OnEnterRoom()
        {
            for (int i = 0;i < currentTriggers.Count;i++)
            {
                if (currentTriggers[i] == null)
                    continue;

                ChoreState requiredChoreState = ChoreManager.Instance.GetChoreState(currentTriggers[i].ChoreRequiredForActivation);

                if (requiredChoreState != ChoreState.Completed)
                    continue;

                if (currentTriggers[i].choreProgressor != null)
                {
                    ChoreState progressorState = currentTriggers[i].choreProgressor.GetChoreState();
                    bool isChoreStateValid = progressorState == ChoreState.Available || progressorState == ChoreState.Completed;

                    if (isChoreStateValid)
                    {
                        currentTriggers[i].choreProgressor?.ProgressChore();
                    }
                }

                if (currentTriggers[i].dynamicThought != null)
                {
                    currentTriggers[i].dynamicThought.PlayThought();
                }
            }
        }

        public void GetRoomCompletion(out int numChoresComplete, out int numTotalChores)
        {
            HashSet<string> searchedChoreID = new HashSet<string>();
            numChoresComplete = 0;
            numTotalChores = 0;

            for (int i = 0; i < roomChores.Length; i++)
            {
                if (searchedChoreID.Contains(roomChores[i].GetChoreID()))
                {
                    continue;
                }

                searchedChoreID.Add(roomChores[i].GetChoreID());
                
                switch (roomChores[i].GetChoreState())
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
