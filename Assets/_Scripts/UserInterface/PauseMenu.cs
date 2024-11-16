using Horror.InputSystem;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface
{
	public class PauseMenu : UIScreen
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
			settingsMenuUI.SetActive(false);

			resumeButton.onClick.AddListener(HandleCancel);
			settingsButton.onClick.AddListener(OpenSettings);
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			shadeOverlay.SetActive(true);
			Time.timeScale = 0f;
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			shadeOverlay.SetActive(false);
			settingsMenuUI.SetActive(false);
			Time.timeScale = 1f;
		}

		public override void HandleCancel()
		{
			if (settingsMenuUI.activeSelf)
			{
				settingsMenuUI.SetActive(false);
			}
			else
			{
				base.HandleCancel();
			}
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