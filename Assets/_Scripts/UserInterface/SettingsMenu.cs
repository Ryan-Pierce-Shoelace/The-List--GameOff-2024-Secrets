using System.Collections.Generic;
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
        [SerializeField] private Slider volumeSlider;
        [SerializeField] private Button closeButton;

        private Resolution[] resolutions;

        private void Awake()
        {
            SetupListeners();
        }

        private void OnEnable()
        {
            SetupListeners();
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

            if (volumeSlider != null)
            {
                volumeSlider.onValueChanged.RemoveAllListeners();
                volumeSlider.onValueChanged.AddListener(SetMasterVolume);
            }

            resolutionDropdown.onValueChanged.RemoveAllListeners();
            resolutionDropdown.onValueChanged.AddListener(ChangeResolution);
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
