using Project.Input;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Horror.InputSystem
{
	[CreateAssetMenu(fileName = "Input Reader SO", menuName = "Input Reader")]
	public class InputReader : ScriptableObject, HorrorInput.IPlayerActions, HorrorInput.IUIActions
	{
		[SerializeField] private InputActionAsset inputActionAsset;

		private HorrorInput input;

		#region Events

		public event UnityAction InteractEvent = delegate { };
		public event UnityAction PauseEvent = delegate { };
		public event UnityAction CancelEvent = delegate { };

		#endregion

		public Vector2 CurrentMove { get; private set; }

		private void OnEnable()
		{
			if (input == null)
			{
				input = new HorrorInput();
				input.Player.SetCallbacks(this);
				input.UI.SetCallbacks(this);
			}

			EnableGameplayInput();
		}

		private void OnDisable()
		{
			DisableAllInput();
		}

		public void EnableGameplayInput()
		{
			input.Player.Enable();
			input.UI.Disable();
		}

		public void EnableUIInput()
		{
			input.Player.Disable();
			input.UI.Enable();
		}

		public void DisableAllInput()
		{
			input.Player.Disable();
			input.UI.Disable();
		}

		#region Player Input Callbacks

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
			if (context.performed)
				PauseEvent?.Invoke();
		}

		#endregion

		#region UI Input Callbacks

		public void OnCancel(InputAction.CallbackContext context)
		{
			if (context.performed)
				CancelEvent?.Invoke();
		}


		public void OnNavigate(InputAction.CallbackContext context)
		{
		}

		public void OnSubmit(InputAction.CallbackContext context)
		{
		}

		public void OnPoint(InputAction.CallbackContext context)
		{
		}

		public void OnClick(InputAction.CallbackContext context)
		{
		}

		public void OnScrollWheel(InputAction.CallbackContext context)
		{
		}

		public void OnMiddleClick(InputAction.CallbackContext context)
		{
		}

		public void OnRightClick(InputAction.CallbackContext context)
		{
		}

		public void OnTrackedDevicePosition(InputAction.CallbackContext context)
		{
		}

		public void OnTrackedDeviceOrientation(InputAction.CallbackContext context)
		{
		}
		#endregion
	}
}