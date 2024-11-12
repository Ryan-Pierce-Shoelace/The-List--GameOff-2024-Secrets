using System.Collections.Generic;
using Shoelace.Audio.XuulSound;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace UserInterface
{
    public class SettingsMenu : MonoBehaviour
    {
        [SerializeField] private AudioMixer masterAudioMixer;
        [SerializeField] private GameObject pauseMenuUI;

        [Header("Toggles")]
        [SerializeField] private TMP_Dropdown resolutionDropdown;
        [SerializeField] private Toggle fullScreenToggle;
        [SerializeField] private Button closeButton;
        
        [SerializeField] private Slider masterVolumeSlider;
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider sfxVolumeSlider;
        

        private Resolution[] resolutions;

        private void Awake()
        {
            SetupListeners();
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
        
        private void SetupVolumeControls()
        {
            VolumeSettings volumeSettings = AudioManager.Instance.VolumeSettings;

            // Remove existing listeners to prevent duplicates
            masterVolumeSlider.onValueChanged.RemoveAllListeners();
            musicVolumeSlider.onValueChanged.RemoveAllListeners();
            sfxVolumeSlider.onValueChanged.RemoveAllListeners();

            // Set initial values
            masterVolumeSlider.value = volumeSettings.Master;
            musicVolumeSlider.value = volumeSettings.Music;
            sfxVolumeSlider.value = volumeSettings.SFX;

            // Add listeners
            masterVolumeSlider.onValueChanged.AddListener(value => {
                volumeSettings.SetMasterVolume(value);
                volumeSettings.SaveSettings();
            });

            musicVolumeSlider.onValueChanged.AddListener(value => {
                volumeSettings.SetMusicVolume(value);
                volumeSettings.SaveSettings();
            });

            sfxVolumeSlider.onValueChanged.AddListener(value => {
                volumeSettings.SetSFXVolume(value);
                volumeSettings.SaveSettings();
            });
        }
        

        private void SetupResolutionDropdown()
        {
            resolutions = Screen.resolutions;

            resolutionDropdown.ClearOptions();

            List<string> options = new List<string>();
            int currentResolutionIndex = 0;
            for (int i = 0; i < resolutions.Length; i++) {
                string option = resolutions[i].width + " x " + resolutions[i].height;
                options.Add(option);

                if (resolutions[i].width == Screen.currentResolution.width &&
                    resolutions[i].height == Screen.currentResolution.height) {
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

        public void SetMasterVolume(float volume)
        {
            masterAudioMixer.SetFloat("MasterVolume", volume);
        }

        public void CloseSettings()
        {
            this.gameObject.SetActive(false);
            pauseMenuUI.SetActive(true);
        }
    }

}
