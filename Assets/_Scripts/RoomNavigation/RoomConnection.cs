using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Horror.RoomNavigation
{
    [System.Serializable]
    public class RoomConnection
    {
        public Doorway doorwayA, doorwayB;

        public RoomConnection(Doorway initDoorway)
        {
            doorwayA = initDoorway;
        }

        public void LinkDoorway(Doorway newDoor)
        {
            if(doorwayA == null)
            {
                doorwayA = newDoor;
                return;
            }
            if(doorwayB == null)
            {
                doorwayB = newDoor;
                return;
            }
                

            // both doors are filled
            Debug.LogError($"Too many doors linked, pair {newDoor.name} to another room");
        }

        public Doorway GetOtherDoor(Doorway startDoor)
        {
            if(startDoor != doorwayA && startDoor != doorwayB)
            {
                Debug.LogError("Can't find door in connection");
                return null;
            }

            if (startDoor == doorwayA)
                return doorwayB;
            else
                return doorwayA;
        }
    }
}
