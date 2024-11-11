using UnityEngine;

namespace Shoelace.Audio.XuulSound
{
    [System.Serializable]
    public class VolumeSettings
    {
        public float Master { get; private set; } = 1f;
        public float Music { get; private set; } = 1f;
        public float SFX { get; private set; } = 1f;

        public event System.Action<VolumeSettings> OnVolumeChanged;

        public void SetMasterVolume(float value)
        {
            Master = Mathf.Clamp01(value);
            OnVolumeChanged?.Invoke(this);
        }

        public void SetMusicVolume(float value)
        {
            Music = Mathf.Clamp01(value);
            OnVolumeChanged?.Invoke(this);
        }

        public void SetSFXVolume(float value)
        {
            SFX = Mathf.Clamp01(value);
            OnVolumeChanged?.Invoke(this);
        }

        public void LoadSettings()
        {
            Master = PlayerPrefs.GetFloat("MasterVolume", 1f);
            Music = PlayerPrefs.GetFloat("MusicVolume", 1f);
            SFX = PlayerPrefs.GetFloat("SFXVolume", 1f);
            OnVolumeChanged?.Invoke(this);
        }

        public void SaveSettings()
        {
            PlayerPrefs.SetFloat("MasterVolume", Master);
            PlayerPrefs.SetFloat("MusicVolume", Music);
            PlayerPrefs.SetFloat("SFXVolume", SFX);
            PlayerPrefs.Save();
        }
    }
}
