using Horror.InputSystem;
using UnityEngine;

namespace UserInterface
{
	public abstract class UIScreen : MonoBehaviour
	{
		[SerializeField] protected InputReader inputReader;

		protected virtual void OnEnable()
		{
			inputReader.EnableUIInput();
			inputReader.CancelEvent += HandleCancel;
		}

		protected virtual void OnDisable()
		{
			inputReader.EnableGameplayInput();
			inputReader.CancelEvent -= HandleCancel;
		}

		protected virtual void HandleCancel()
		{
			gameObject.SetActive(false);
		}
	}
}