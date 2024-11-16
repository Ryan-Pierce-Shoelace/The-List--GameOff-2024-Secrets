using Horror.InputSystem;
using UnityEngine;

namespace UserInterface
{
	public abstract class UIScreen : MonoBehaviour
	{
		[SerializeField] protected InputReader inputReader;

		protected Canvas parentCanvas;

		protected virtual void Awake()
		{
			parentCanvas = GetComponentInParent<Canvas>();
		}

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

		public virtual void HandleCancel()
		{
			if (parentCanvas != null)
			{
				parentCanvas.gameObject.SetActive(false);
			}
			else
			{
				gameObject.SetActive(false);
				Debug.LogWarning($"Parent Canvas not found for {gameObject.name}, falling back to disabling GameObject directly.", this);
			}
		}
	}
}