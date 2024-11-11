using System;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

namespace Shoelace.Audio.XuulSound
{
	public class AudioManager : MonoBehaviour
	{
		public static AudioManager Instance { get; private set; }

		public VolumeSettings VolumeSettings => volumeSettings;


		[Header("Initial Setup")]
		[SerializeField] private SoundConfig startingMusic;

		[SerializeField] private bool playMusicOnStart = true;

		[Header("Volume Controls")]
		[SerializeField, Range(0, 1)] private float masterVolume = 1f;

		[SerializeField, Range(0, 1)] private float musicVolume = 1f;
		[SerializeField, Range(0, 1)] private float sfxVolume = 1f;

		[Header("Fade Settings")]
		[SerializeField] private float defaultFadeTime = 2f;


		private readonly Dictionary<string, ISoundPlayer> activeSounds = new();
		private MusicSystem musicSystem;

		private VolumeSettings volumeSettings;


		#region Setup

		private void Awake()
		{
			if (Instance != null)
			{
				Debug.LogError("More than one audio manager in the scene");
			}

			Instance = this;
			DontDestroyOnLoad(gameObject);

			InitializeSystem();
		}

		private void Start()
		{
			if (playMusicOnStart && startingMusic != null)
			{
				PlayMusic(startingMusic);
			}

			UpdateAllVolumes();
		}


		private void InitializeSystem()
		{
			volumeSettings = new VolumeSettings();
			volumeSettings.OnVolumeChanged += HandleVolumeChanged;
			volumeSettings.LoadSettings();

			musicSystem = new MusicSystem();
			UpdateMusicVolume();
		}

		#endregion

		#region Volume Controls

		private void HandleVolumeChanged(VolumeSettings settings)
		{
			UpdateAllVolumes();
		}

		private void UpdateAllVolumes()
		{
			UpdateMusicVolume();
			UpdateSFXVolume();
		}

		private void UpdateSFXVolume()
		{
			float effectiveVolume = masterVolume * sfxVolume;
			foreach (ISoundPlayer sound in activeSounds.Values)
			{
				sound.SetVolume(effectiveVolume);
			}
		}

		private void UpdateMusicVolume()
		{
			if (musicSystem != null)
			{
				musicSystem.Volume = masterVolume * musicVolume;
			}
		}

		#endregion


		public void PlayOneShot(SoundConfig config, Vector3 position = default)
		{
			RuntimeManager.PlayOneShot(config.EventRef, position);
		}

		public ISoundPlayer CreateSound(SoundConfig config, Transform parent = null)
		{
			ISoundPlayer player = config.Is3D ? new AttachedSoundPlayer(config, parent) : new SimpleSoundPlayer(config);

			player.SetVolume(masterVolume * sfxVolume);

			string id = Guid.NewGuid().ToString();
			activeSounds[id] = player;
			return player;
		}

		public void PlayMusic(SoundConfig music, float fadeTime = 2f)
		{
			musicSystem.PlayMusic(music, fadeTime);
		}

		public void StopMusic(float fadeTime = 2f)
		{
			musicSystem.StopMusic(fadeTime);
		}


		public void StopAllSounds()
		{
			foreach (ISoundPlayer sound in activeSounds.Values)
			{
				sound.Stop();
			}
		}

		private void OnDestroy()
		{
			if (Instance == this)
			{
				Instance = null;
			}

			foreach (ISoundPlayer sound in activeSounds.Values)
			{
				sound.Dispose();
			}

			activeSounds.Clear();

			musicSystem?.Dispose();
		}
	}
}