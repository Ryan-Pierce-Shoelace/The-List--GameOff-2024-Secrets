using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UserInterface
{
	public class PauseMenu : MonoBehaviour
	{
		public bool IsGamePaused = false;

		[SerializeField] private GameObject shadeOverlay;
		[SerializeField] private GameObject pauseMenuUI;
		[SerializeField] private GameObject settingsMenuUI;

		[Header("Buttons")]
		[SerializeField] private Button resumeButton;

		[SerializeField] private Button saveButton;
		[SerializeField] private Button loadButton;
		[SerializeField] private Button settingsButton;
		[SerializeField] private Button quitButton;


		private void Start()
		{
			pauseMenuUI.SetActive(false);
			settingsMenuUI.SetActive(false);
			shadeOverlay.SetActive(false);
			AddHoverListeners(resumeButton);
			AddHoverListeners(saveButton);
			AddHoverListeners(loadButton);
			AddHoverListeners(settingsButton);
			AddHoverListeners(quitButton);
		}


		#region Menu Options

		public void SaveGame()
		{
			//
		}

		public void LoadGame()
		{
		//
		}

		public void OpenSettings()
		{
			settingsMenuUI.SetActive(true);
			pauseMenuUI.SetActive(false);
		}

		public void QuitGame()
		{
			Debug.Log("Quitting game...");
		}

		#endregion



		private void AddHoverListeners(Button button)
		{
			EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>() ?? button.gameObject.AddComponent<EventTrigger>();

			EventTrigger.Entry enterEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
			enterEntry.callback.AddListener((data) => { OnPointerEnter(button); });
			trigger.triggers.Add(enterEntry);

			EventTrigger.Entry exitEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
			exitEntry.callback.AddListener((data) => { OnPointerExit(button); });
			trigger.triggers.Add(exitEntry);
		}

		private void OnPointerEnter(Button button)
		{
			Debug.Log($"Pointer entered {button.name}");
		}

		private void OnPointerExit(Button button)
		{
			Debug.Log($"Pointer exited {button.name}");
		}
		public void TogglePause()
		{
			if (IsGamePaused)
			{
				Resume();
			}
			else
			{
				Pause();
			}
		}

		public void Resume()
		{
			pauseMenuUI.SetActive(false);
			Time.timeScale = 1f;
			IsGamePaused = false;
			shadeOverlay.SetActive(false);
		}

		private void Pause()
		{
			shadeOverlay.SetActive(true);
			pauseMenuUI.SetActive(true);
			Time.timeScale = 0f;
			IsGamePaused = true;
		}


		public void CloseSettings()
		{
			settingsMenuUI.SetActive(false);
			pauseMenuUI.SetActive(true);
		}
	}
}