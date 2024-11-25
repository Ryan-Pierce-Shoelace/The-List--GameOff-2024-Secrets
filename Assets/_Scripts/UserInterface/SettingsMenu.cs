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

		private List<Vector2Int> availableResolutions;

		protected override void Awake()
		{
			parentCanvas = GetComponentInParent<Canvas>();
			SetupAudioSliders();
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			InitializeUI();
			LoadAudioSettings();
			InitializeResolutions();
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

		private void InitializeResolutions()
		{
			availableResolutions = Screen.resolutions
				.Select(r => new Vector2Int(r.width, r.height))
				.Distinct()
				.OrderByDescending(r => r.x * r.y)
				.ToList();

			resolutionDropdown.ClearOptions();

			var options = availableResolutions.Select(r => $"{r.x} x {r.y}").ToList();
			resolutionDropdown.AddOptions(options);

			var currentResolution = new Vector2Int(Screen.width, Screen.height);
			var currentIndex = availableResolutions.FindIndex(r => r == currentResolution);
			resolutionDropdown.value = currentIndex;

			resolutionDropdown.onValueChanged.RemoveAllListeners();
			resolutionDropdown.onValueChanged.AddListener(UpdateResolution);
		}

		private void UpdateResolution(int index)
		{
			if (index < 0 || index >= availableResolutions.Count) return;
			var newResolution = availableResolutions[index];
			Screen.SetResolution(newResolution.x, newResolution.y, Screen.fullScreenMode);
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