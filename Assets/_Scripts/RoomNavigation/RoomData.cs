using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

namespace Horror.RoomNavigation
{
    public class RoomData : MonoBehaviour
    {
        public string RoomName;

        [SerializeField] private int tempTestNumberOfRoomChores;
        [SerializeField] private CinemachineVirtualCamera roomCamera;
        public Dictionary<string, bool> roomChores;

        private void Start()
        {
            roomChores = new Dictionary<string, bool>();

            for (int i = 0; i < tempTestNumberOfRoomChores; i++)
            {
                roomChores.Add($"chore{i}", false);
            }
        }

        public void GetRoomCompletion(out int numChoresComplete, out int numTotalChores)
        {
            numChoresComplete = Random.Range(0, roomChores.Count); //TODO Fix this with proper info
            numTotalChores = roomChores.Count;
        }
        public void ToggleRoomCamera(bool toggle)
        {
            roomCamera.gameObject.SetActive(toggle);
        }

    }
}
