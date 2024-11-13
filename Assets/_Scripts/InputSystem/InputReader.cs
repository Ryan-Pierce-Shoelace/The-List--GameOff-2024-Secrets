using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Project.Input
{
    [CreateAssetMenu(fileName = "Input Reader SO", menuName = "Input Reader")]
    public class InputReader : ScriptableObject, HorrorInput.IPlayerActions
    {
        HorrorInput input;
        [SerializeField]
        private InputActionAsset inputActionAsset;

        #region Input Events

        public event UnityAction InteractEvent = delegate { };
        public event UnityAction PauseEvent = delegate { };


        #endregion

        [FormerlySerializedAs("currentMove")] public Vector2 CurrentMove;

        //public bool isGamepad;

        private void OnEnable()
        {
            input = new HorrorInput();
            input.Player.SetCallbacks(this);
            //input.Menu.SetCallbacks(this) <- can add something like this in the future9
            input.Player.Enable();

            //input.UI.Disable(); will need to toggle active action maps once we add more to the system.
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            CurrentMove = context.ReadValue<Vector2>();
        }
        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.performed)
                InteractEvent?.Invoke();
        }
        public void OnPause(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                PauseEvent.Invoke();
        }
    }
}

