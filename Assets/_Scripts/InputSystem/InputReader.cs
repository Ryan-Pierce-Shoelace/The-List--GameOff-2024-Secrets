using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Project.Input
{
    [CreateAssetMenu(fileName = "Input Reader SO", menuName = "Input Reader")]
    public class InputReader : ScriptableObject, HorrorInput.IPlayerActions
    {
        HorrorInput input;
        [SerializeField]
        private InputActionAsset inputActionAsset;

        #region Input Events

        public event UnityAction interactEvent = delegate { };
        public event UnityAction pauseEvent = delegate { };


        #endregion

        public Vector2 currentMove;

        //public bool isGamepad;

        private void OnEnable()
        {
            if (input == null)
            {
                input = new HorrorInput();
                input.Player.SetCallbacks(this);


                //input.Menu.SetCallbacks(this) <- can add something like this in the future9
            }

            input.Player.Enable();

            //input.UI.Disable(); will need to toggle active action maps once we add more to the system.
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            currentMove = context.ReadValue<Vector2>();
        }
        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.performed)
                interactEvent?.Invoke();
        }
        public void OnPause(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                pauseEvent.Invoke();
        }
    }
}

