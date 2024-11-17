using UnityEngine;

namespace Horror.RoomNavigation
{
    public class Doorway : MonoBehaviour
    {
        public RoomData RootRoom;
        public RoomLinkKey LinkKey;

        [SerializeField] private Transform exitPoint;
        [SerializeField] private GameObject closedDoor;

        

        public Vector2 DoorExitPos => exitPoint.position;
        public bool IsDoorClosed => closedDoor.activeSelf;

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

        

        public void SetDoorActiveState(bool state)
        {
            closedDoor.SetActive(state);
        }
    }
}
