using System.Collections.Generic;
using Shoelace.Audio.XuulSound;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UserInterface;

namespace Horror.UserInterface
{
	public class SettingsMenu : UIScreen
	{
		[SerializeField] private GameObject pauseMenu;
		[SerializeField] private bool isMainMenuScreen = false;

		[Header("Toggles")]
		[SerializeField] private TMP_Dropdown resolutionDropdown;

		[SerializeField] private Toggle fullScreenToggle;
		[SerializeField] private Button closeButton;

		[SerializeField] private Slider masterVolumeSlider;
		[SerializeField] private Slider musicVolumeSlider;
		[SerializeField] private Slider sfxVolumeSlider;
		[SerializeField] private Slider ambienceVolumeSlider;


		private Resolution[] resolutions;


		protected override void Awake()
		{
			parentCanvas = GetComponentInParent<Canvas>();
			ConfigureSliders();
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			SetupListeners();
			LoadAndApplyAudioSettings();
			SetupResolutionDropdown();
		}

		public override void HandleCancel()
		{
			if (isMainMenuScreen)
			{
				parentCanvas.gameObject.SetActive(false);
				return;
			}
			else
			{
				gameObject.SetActive(false);
				pauseMenu.gameObject.SetActive(true);
			}
		}


		private void SetupListeners()
		{
			closeButton.onClick.RemoveAllListeners();
			closeButton.onClick.AddListener(HandleCancel);

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
			SetupSlider(masterVolumeSlider);
			SetupSlider(musicVolumeSlider);
			SetupSlider(sfxVolumeSlider);
			SetupSlider(ambienceVolumeSlider);
		}

		private void SetupSlider(Slider slider)
		{
			if (slider == null) return;

			slider.minValue = 0f;
			slider.maxValue = 1f;
			slider.value = 1f;
		}

		private void LoadAndApplyAudioSettings()
		{
			if (AudioManager.Instance == null)
			{
				Debug.LogError("AudioManager instance not found!");
				return;
			}

			if (masterVolumeSlider != null)
			{
				masterVolumeSlider.value = AudioManager.Instance.MasterVolume;
				masterVolumeSlider.onValueChanged.RemoveAllListeners();
				masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
			}

			if (musicVolumeSlider != null)
			{
				musicVolumeSlider.value = AudioManager.Instance.MusicVolume;
				musicVolumeSlider.onValueChanged.RemoveAllListeners();
				musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
			}

			if (sfxVolumeSlider != null)
			{
				sfxVolumeSlider.value = AudioManager.Instance.SFXVolume;
				sfxVolumeSlider.onValueChanged.RemoveAllListeners();
				sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
			}


			if (ambienceVolumeSlider != null)
			{
				ambienceVolumeSlider.value = AudioManager.Instance.AmbientVolume;
				ambienceVolumeSlider.onValueChanged.RemoveAllListeners();
				ambienceVolumeSlider.onValueChanged.AddListener(OnAmbienceVolumeChanged);
			}
		}


		private void OnMasterVolumeChanged(float value)
		{
			if (AudioManager.Instance != null)
			{
				AudioManager.Instance.MasterVolume = value;
			}
		}

		private void OnMusicVolumeChanged(float value)
		{
			if (AudioManager.Instance != null)
			{
				AudioManager.Instance.MusicVolume = value;
			}
		}

		private void OnSFXVolumeChanged(float value)
		{
			if (AudioManager.Instance != null)
			{
				AudioManager.Instance.SFXVolume = value;
			}
		}

		private void OnAmbienceVolumeChanged(float value)
		{
			if (AudioManager.Instance != null)
			{
				AudioManager.Instance.AmbientVolume = value;
			}
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
			if (isMainMenuScreen)
			{
				parentCanvas.gameObject.SetActive(false);
				return;
			}
			else
			{
				gameObject.SetActive(false);
				pauseMenu.gameObject.SetActive(true);
			}
		}
	}
}