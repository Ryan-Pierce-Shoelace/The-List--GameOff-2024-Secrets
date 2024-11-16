using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Horror.RoomNavigation
{
    public class Doorway : MonoBehaviour
    {
        public RoomData RootRoom;
        public RoomLinkKey LinkKey;

        [SerializeField] private Transform exitPoint;

        public Vector2 DoorExitPos => exitPoint.position;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.attachedRigidbody.TryGetComponent(out PlayerNavigator navigator))
            {
                navigator.CurrentDoorway = this;
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.attachedRigidbody.TryGetComponent(out PlayerNavigator navigator))
            {
                navigator.CurrentDoorway = null;
            }
        }
    }
}
