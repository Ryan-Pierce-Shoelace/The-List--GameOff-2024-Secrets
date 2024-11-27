using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UserInterface;
using Shoelace.Audio.XuulSound;

namespace Horror.UserInterface
{
	public class SettingsMenu : UIScreen
	{
		[SerializeField] private GameObject pauseMenu;
		[SerializeField] private bool isMainMenuScreen;
		[SerializeField] private TMP_Dropdown resolutionDropdown;
		[SerializeField] private Toggle fullScreenToggle;
		[SerializeField] private Button closeButton;
		[SerializeField] private Slider masterVolumeSlider;
		[SerializeField] private Slider musicVolumeSlider;
		[SerializeField] private Slider sfxVolumeSlider;
		[SerializeField] private Slider ambienceVolumeSlider;

		private Resolution[] resolutions;
		private List<Resolution> filteredResolutions;

		protected override void Awake()
		{
			parentCanvas = GetComponentInParent<Canvas>();
			SetupAudioSliders();
		}

		private void Start()
		{
			resolutions = Screen.resolutions;
			filteredResolutions = resolutions.Distinct().ToList();
			resolutionDropdown.ClearOptions();
			List<string> options = new List<string>();

			int currentResolutionIndex = 0;


			for (int i = 0; i < filteredResolutions.Count; i++)
			{
				string option = $"{filteredResolutions[i].width} x {filteredResolutions[i].height} @ {filteredResolutions[i].refreshRate}Hz";
				options.Add(option);


				if (filteredResolutions[i].width == Screen.currentResolution.width &&
				    filteredResolutions[i].height == Screen.currentResolution.height)
				{
					currentResolutionIndex = i;
				}
			}


			resolutionDropdown.AddOptions(options);
			resolutionDropdown.value = currentResolutionIndex;
			resolutionDropdown.RefreshShownValue();


			resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			InitializeUI();
			LoadAudioSettings();
		}

		public override void HandleCancel()
		{
			if (isMainMenuScreen)
			{
				parentCanvas.gameObject.SetActive(false);
				return;
			}

			gameObject.SetActive(false);
			pauseMenu.gameObject.SetActive(true);
		}

		private void InitializeUI()
		{
			closeButton.onClick.RemoveAllListeners();
			closeButton.onClick.AddListener(HandleCancel);

			if (fullScreenToggle != null)
			{
				fullScreenToggle.isOn = Screen.fullScreenMode != FullScreenMode.Windowed;
				fullScreenToggle.onValueChanged.RemoveAllListeners();
				fullScreenToggle.onValueChanged.AddListener(UpdateScreenMode);
			}
		}

		private void SetupAudioSliders()
		{
			foreach (var slider in new[] { masterVolumeSlider, musicVolumeSlider, sfxVolumeSlider, ambienceVolumeSlider })
			{
				if (slider != null)
				{
					slider.minValue = 0f;
					slider.maxValue = 1f;
					slider.value = 1f;
				}
			}
		}

		private void LoadAudioSettings()
		{
			if (!AudioManager.Instance) return;

			SetupAudioSlider(masterVolumeSlider, AudioManager.Instance.MasterVolume, OnMasterVolumeChanged);
			SetupAudioSlider(musicVolumeSlider, AudioManager.Instance.MusicVolume, OnMusicVolumeChanged);
			SetupAudioSlider(sfxVolumeSlider, AudioManager.Instance.SFXVolume, OnSFXVolumeChanged);
			SetupAudioSlider(ambienceVolumeSlider, AudioManager.Instance.AmbientVolume, OnAmbienceVolumeChanged);
		}

		private void SetupAudioSlider(Slider slider, float initialValue, UnityEngine.Events.UnityAction<float> callback)
		{
			if (slider == null) return;
			slider.value = initialValue;
			slider.onValueChanged.RemoveAllListeners();
			slider.onValueChanged.AddListener(callback);
		}

		private void OnResolutionChanged(int index)
		{
			Resolution resolution = filteredResolutions[index];
			Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);


			PlayerPrefs.SetInt("ResolutionIndex", index);
			PlayerPrefs.Save();
		}


		private void LoadSavedResolution()
		{
			if (PlayerPrefs.HasKey("ResolutionIndex"))
			{
				int savedIndex = PlayerPrefs.GetInt("ResolutionIndex");
				if (savedIndex < filteredResolutions.Count)
				{
					resolutionDropdown.value = savedIndex;
					OnResolutionChanged(savedIndex);
				}
			}
		}

		private void UpdateScreenMode(bool isFullScreen)
		{
			Screen.fullScreenMode = isFullScreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
		}

		private void OnMasterVolumeChanged(float value)
		{
			if (AudioManager.Instance) AudioManager.Instance.MasterVolume = value;
		}

		private void OnMusicVolumeChanged(float value)
		{
			if (AudioManager.Instance) AudioManager.Instance.MusicVolume = value;
		}

		private void OnSFXVolumeChanged(float value)
		{
			if (AudioManager.Instance) AudioManager.Instance.SFXVolume = value;
		}

		private void OnAmbienceVolumeChanged(float value)
		{
			if (AudioManager.Instance) AudioManager.Instance.AmbientVolume = value;
		}
	}
}