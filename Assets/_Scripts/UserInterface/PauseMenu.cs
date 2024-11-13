using Project.Input;
using Project.Input.InputSystem;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface
{
	public class PauseMenu : MonoBehaviour
	{
		public bool IsGamePaused = false;

		[SerializeField] private InputReader inputReader;


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
		}

		private void OnEnable()
		{
			inputReader.PauseEvent += HandlePauseInput;
		}

		private void OnDisable()
		{
			inputReader.PauseEvent -= HandlePauseInput;
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


		private void HandlePauseInput()
		{
			if (settingsMenuUI.activeSelf)
			{
				CloseSettings();
			}
			else
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
		}


		public void Resume()
		{
			pauseMenuUI.SetActive(false);
			settingsMenuUI.SetActive(false);
			shadeOverlay.SetActive(false);
			Time.timeScale = 1f;
			IsGamePaused = false;
		}

		private void Pause()
		{
			shadeOverlay.SetActive(true);
			pauseMenuUI.SetActive(true);
			settingsMenuUI.SetActive(false);
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