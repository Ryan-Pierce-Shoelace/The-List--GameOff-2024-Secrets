using Horror.InputSystem;
using Horror.Player;
using Sirenix.OdinInspector;
using System.Threading.Tasks;
using UnityEngine;
using Utilities;

namespace Horror.RoomNavigation
{
    public class PlayerNavigator : MonoBehaviour
    {
        private Doorway current;
        [ReadOnly] public Doorway CurrentDoorway
        {
            get
            {
                return current;
            }
            set
            {
                if (current != null && value == null)
                    playerSprite.color = Color.white;

                current = value;
            }
        }

        [SerializeField] private InputReader input;

        [SerializeField] private float doorWayDepth;
        [SerializeField] private SpriteRenderer playerSprite;
        [SerializeField] private PlayerMovement movement;

        private bool transitioning;

        private void LateUpdate()
        {
            if (CurrentDoorway != null)
            {
                CalculateNavigationProgress();
            }
        }

        private void CalculateNavigationProgress()
        {
            float yDifference = transform.position.y - CurrentDoorway.transform.position.y;
            if (yDifference <= 0)
                return;
            else
            {
                float navigationT = Mathf.InverseLerp(0, doorWayDepth, yDifference);
                playerSprite.color = Color.Lerp(Color.white, Color.black, navigationT);

                if (navigationT >= 1 && !transitioning)
                {
                    transitioning = true;
                    MoveToNewRoom(CurrentDoorway, NavigationManager.HouseNavigation[CurrentDoorway.LinkKey].GetOtherDoor(CurrentDoorway));
                }
            }
        }

        private async void MoveToNewRoom(Doorway current, Doorway nextDoor)
        {
            input.DisableAllInput();

            await FadeTransition.Instance.ToggleFadeTransition(true, .3f);

            current.RootRoom.ToggleRoomCamera(false);
            nextDoor.RootRoom.ToggleRoomCamera(true);

            //current.TravelSFX.Play();
            movement.ToggleInput(false);
            transform.position = nextDoor.DoorExitPos + (Vector2.up * doorWayDepth);

            RoomData newRoom = nextDoor.RootRoom;

            newRoom.OnEnterRoom();
            newRoom.GetRoomCompletion(out int completed, out int total);
            StaticEvents.DisplayRoomData(newRoom.RoomName, completed, total);


            Task fadeIn = FadeTransition.Instance.ToggleFadeTransition(false, .3f);
            Task walkIn = movement.AutoWalkToPosition(nextDoor.DoorExitPos);
            Task[] moveIntoRoom = new Task[]
            {
                fadeIn, walkIn
            };

            await Task.WhenAll(moveIntoRoom);

            
            movement.ToggleInput(true);
            input.EnableGameplayInput();
            transitioning = false;
        }

        public async void ForceMoveToNewRoom(Doorway escapeDoor)
        {
            transitioning = true;

            Doorway dest = NavigationManager.HouseNavigation[escapeDoor.LinkKey].GetOtherDoor(escapeDoor);

            await FadeTransition.Instance.ToggleFadeTransition(true, .3f);

            escapeDoor.RootRoom.ToggleRoomCamera(false);
            dest.RootRoom.ToggleRoomCamera(true);

            transform.position = dest.DoorExitPos;

            RoomData newRoom = dest.RootRoom;

            newRoom.OnEnterRoom();
            newRoom.GetRoomCompletion(out int completed, out int total);
            StaticEvents.DisplayRoomData(newRoom.RoomName, completed, total);


            await FadeTransition.Instance.ToggleFadeTransition(false, .3f);

            movement.ToggleInput(true);
            input.EnableGameplayInput();
            transitioning = false;
        }
    }
}
