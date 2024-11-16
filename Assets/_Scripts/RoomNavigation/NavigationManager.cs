using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Horror.RoomNavigation
{
    public class NavigationManager : MonoBehaviour
    {
        public static Dictionary<RoomLinkKey, RoomConnection> HouseNavigation;

        private void Start()
        {
            Doorway[] allDoors = FindObjectsOfType<Doorway>();

            HouseNavigation = new Dictionary<RoomLinkKey, RoomConnection>();
            for (int i = 0; i < allDoors.Length; i++)
            {
                if (allDoors[i].LinkKey == null)
                {
                    Debug.Log($"{allDoors[i].name} has no Room Link");
                    continue;
                }

                if (HouseNavigation.ContainsKey(allDoors[i].LinkKey))
                {
                    HouseNavigation[allDoors[i].LinkKey].LinkDoorway(allDoors[i]);
                }
                else
                {
                    HouseNavigation.Add(allDoors[i].LinkKey, new RoomConnection(allDoors[i]));
                }
            }
        }

        public static async void MoveToRoom(Doorway sourceDoor, PlayerNavigator player)
        {
            Doorway newDoor = HouseNavigation[sourceDoor.LinkKey].GetOtherDoor(sourceDoor);

            sourceDoor.RootRoom.ToggleRoomCamera(false);
            newDoor.RootRoom.ToggleRoomCamera(true);
        }
    }
}
