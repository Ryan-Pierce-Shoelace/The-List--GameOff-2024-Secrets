using Horror.InputSystem;
using UnityEngine;

namespace UserInterface
{
	public class PauseController : MonoBehaviour
	{
		[SerializeField] private InputReader inputReader;
		[SerializeField] private PauseMenu pauseMenuScreen;

		private void Start()
		{
			inputReader.EnableGameplayInput();
		}

		private void OnEnable()
		{
			inputReader.PauseEvent += HandlePauseInput;
		}

		private void OnDisable()
		{
			inputReader.PauseEvent -= HandlePauseInput;
		}

		private void HandlePauseInput()
		{
			pauseMenuScreen.gameObject.SetActive(true);
		}
	}
}