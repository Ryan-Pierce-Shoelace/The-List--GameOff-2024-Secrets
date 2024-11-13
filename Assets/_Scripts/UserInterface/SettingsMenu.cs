using System.Collections.Generic;
using Shoelace.Audio.XuulSound;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface
{
	public class SettingsMenu : MonoBehaviour
	{
		[SerializeField] private GameObject pauseMenuUI;

		[Header("Toggles")]
		[SerializeField] private TMP_Dropdown resolutionDropdown;

		[SerializeField] private Toggle fullScreenToggle;
		[SerializeField] private Button closeButton;

		[SerializeField] private Slider masterVolumeSlider;
		[SerializeField] private Slider musicVolumeSlider;
		[SerializeField] private Slider sfxVolumeSlider;
		[SerializeField] private Slider ambienceVolumeSlider;


		private Resolution[] resolutions;


		private void Awake()
		{
			ConfigureSliders();
		}

		private void OnEnable()
		{
			SetupListeners();
			SetupVolumeControls();
			SetupResolutionDropdown();
		}

		private void SetupListeners()
		{
			closeButton.onClick.RemoveAllListeners();
			closeButton.onClick.AddListener(CloseSettings);

			if (fullScreenToggle != null)
			{
				fullScreenToggle.onValueChanged.RemoveAllListeners();
				fullScreenToggle.onValueChanged.AddListener(SetFullScreen);
			}


			resolutionDropdown.onValueChanged.RemoveAllListeners();
			resolutionDropdown.onValueChanged.AddListener(ChangeResolution);
		}

		private void ConfigureSliders()
		{
			masterVolumeSlider.minValue = 0f;
			masterVolumeSlider.maxValue = 1f;
			masterVolumeSlider.value = 1f;

			musicVolumeSlider.minValue = 0f;
			musicVolumeSlider.maxValue = 1f;
			musicVolumeSlider.value = 1f;

			sfxVolumeSlider.minValue = 0f;
			sfxVolumeSlider.maxValue = 1f;
			sfxVolumeSlider.value = 1f;

			ambienceVolumeSlider.minValue = 0f;
			ambienceVolumeSlider.maxValue = 1f;
			ambienceVolumeSlider.value = 1f;
		}

		private void SetupVolumeControls()
		{
			if (AudioManager.Instance == null)
			{
				Debug.LogError("AudioManager instance not found!");
				return;
			}

			masterVolumeSlider.value = AudioManager.Instance.MasterVolume;
			musicVolumeSlider.value = AudioManager.Instance.MusicVolume;
			sfxVolumeSlider.value = AudioManager.Instance.SFXVolume;
			ambienceVolumeSlider.value = AudioManager.Instance.AmbientVolume;

			masterVolumeSlider.onValueChanged.RemoveAllListeners();
			musicVolumeSlider.onValueChanged.RemoveAllListeners();
			sfxVolumeSlider.onValueChanged.RemoveAllListeners();
			ambienceVolumeSlider.onValueChanged.RemoveAllListeners();

			masterVolumeSlider.onValueChanged.AddListener((value) => AudioManager.Instance.MasterVolume = value);
			musicVolumeSlider.onValueChanged.AddListener((value) => AudioManager.Instance.MusicVolume = value);
			sfxVolumeSlider.onValueChanged.AddListener((value) => AudioManager.Instance.SFXVolume = value);
			ambienceVolumeSlider.onValueChanged.AddListener((value) => AudioManager.Instance.AmbientVolume = value);
		}


		private void SetupResolutionDropdown()
		{
			resolutions = Screen.resolutions;

			resolutionDropdown.ClearOptions();

			List<string> options = new List<string>();
			int currentResolutionIndex = 0;
			for (int i = 0; i < resolutions.Length; i++)
			{
				string option = resolutions[i].width + " x " + resolutions[i].height;
				options.Add(option);

				if (resolutions[i].width == Screen.currentResolution.width &&
				    resolutions[i].height == Screen.currentResolution.height)
				{
					currentResolutionIndex = i;
				}
			}

			options = RemoveDuplicates(options);

			resolutionDropdown.AddOptions(options);
			resolutionDropdown.value = currentResolutionIndex;
			resolutionDropdown.RefreshShownValue();
		}

		private List<string> RemoveDuplicates(List<string> options)
		{
			HashSet<string> uniqueOptions = new HashSet<string>(options);
			return new List<string>(uniqueOptions);
		}


		private void ChangeResolution(int index)
		{
			Resolution resolution = resolutions[index];
			Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
		}

		public void SetFullScreen(bool isFullScreen)
		{
			Screen.fullScreen = isFullScreen;
		}


		public void CloseSettings()
		{
			this.gameObject.SetActive(false);
			pauseMenuUI.SetActive(true);
		}
	}
}