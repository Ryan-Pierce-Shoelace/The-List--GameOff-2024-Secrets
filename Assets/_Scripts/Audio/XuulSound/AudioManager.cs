using System;
using System.Collections.Generic;
using System.Linq;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace Shoelace.Audio.XuulSound
{
	public class AudioManager : MonoBehaviour
	{
		public static AudioManager Instance { get; private set; }

		[Header("Initial Setup")]
		[SerializeField] private SoundConfig startingMusic;

		[SerializeField] private bool playMusicOnStart = true;

		private Bus masterBus;
		private Bus musicBus;
		private Bus sfxBus;
		private Bus ambientBus;

		[SerializeField] private float masterVolume = 1f;
		[SerializeField] private float musicVolume = 1f;
		[SerializeField] private float sfxVolume = 1f;
		[SerializeField] private float ambientVolume = 1f;

		[Header("Fade Settings")]
		[SerializeField] private float defaultFadeTime = 2f;

		private Dictionary<string, ISoundPlayer> activeSounds;
		private HashSet<SoundEmitter> activeEmitters;
		private MusicSystem musicSystem;

		#region Setup

		private void Awake()
		{
			if (Instance != null)
			{
				Debug.LogError("More than one audio manager in the scene");
				Destroy(gameObject);
				return;
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
			activeSounds = new Dictionary<string, ISoundPlayer>();
			activeEmitters = new HashSet<SoundEmitter>();
			musicSystem = new MusicSystem();
			//
			// masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
			// musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
			// sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
			// ambientVolume = PlayerPrefs.GetFloat("AmbientVolume", 1f);

			masterBus = RuntimeManager.GetBus("bus:/");
			musicBus = RuntimeManager.GetBus("bus:/Music");
			sfxBus = RuntimeManager.GetBus("bus:/SFX");
			ambientBus = RuntimeManager.GetBus("bus:/Ambience");

			UpdateAllVolumes();
		}

		#endregion

		#region Volume Controls

		public float MasterVolume
		{
			get => masterVolume;
			set
			{
				masterVolume = Mathf.Clamp01(value);
				PlayerPrefs.SetFloat("MasterVolume", masterVolume);
				PlayerPrefs.Save();
				UpdateAllVolumes();
			}
		}

		public float MusicVolume
		{
			get => musicVolume;
			set
			{
				musicVolume = Mathf.Clamp01(value);
				PlayerPrefs.SetFloat("MusicVolume", musicVolume);
				PlayerPrefs.Save();
				UpdateAllVolumes();
			}
		}

		public float SFXVolume
		{
			get => sfxVolume;
			set
			{
				sfxVolume = Mathf.Clamp01(value);
				PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
				PlayerPrefs.Save();
				UpdateAllVolumes();
			}
		}

		public float AmbientVolume
		{
			get => ambientVolume;
			set
			{
				ambientVolume = Mathf.Clamp01(value);
				PlayerPrefs.SetFloat("AmbientVolume", ambientVolume);
				PlayerPrefs.Save();
				UpdateAllVolumes();
			}
		}


		private void UpdateAllVolumes()
		{
			try
			{
				float masterVolumeDB = ConvertToFMODVolume(masterVolume);
				float musicVolumeDB = ConvertToFMODVolume(musicVolume);
				float sfxVolumeDB = ConvertToFMODVolume(sfxVolume);
				float ambientVolumeDB = ConvertToFMODVolume(ambientVolume);

				masterBus.setVolume(masterVolumeDB);
				musicBus.setVolume(musicVolumeDB);
				sfxBus.setVolume(sfxVolumeDB);
				ambientBus.setVolume(ambientVolumeDB);

				// Debug.Log($"Updated volumes - Master: {masterVolumeDB}, Music: {musicVolumeDB}, " +
				//           $"SFX: {sfxVolumeDB}, Ambient: {ambientVolumeDB}");
			}
			catch (Exception e)
			{
				Debug.LogError($"Error setting FMOD volumes: {e.Message}");
			}
		}

		private float ConvertToFMODVolume(float sliderValue)
		{
			if (sliderValue <= 0) return 0.0001f;
			return sliderValue * sliderValue;
		}

		#endregion

		#region Sound Playback

		public void PlayOneShot(SoundConfig config, Vector3 position = default)
		{
			RuntimeManager.PlayOneShot(config.EventRef, position);
		}

		public ISoundPlayer CreateSound(SoundConfig config, Transform parent = null)
		{
			ISoundPlayer player = config.Is3D ? new AttachedSoundPlayer(config, parent) : new SimpleSoundPlayer(config);

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

		#endregion

		#region Emitter Management

		public void RegisterEmitter(SoundEmitter emitter)
		{
			activeEmitters.Add(emitter);
		}

		public void UnregisterEmitter(SoundEmitter emitter)
		{
			if (emitter != null)
			{
				activeEmitters.Remove(emitter);
			}
		}

		#endregion

		#region Cleanup

		public void StopAllSounds()
		{
			foreach (ISoundPlayer sound in activeSounds.Values)
			{
				sound.Stop();
			}

			foreach (SoundEmitter emitter in activeEmitters)
			{
				emitter.Stop();
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

			foreach (SoundEmitter emitter in activeEmitters.Where(emitter => emitter != null))
			{
				Destroy(emitter.gameObject);
			}

			activeEmitters.Clear();

			musicSystem?.Dispose();
		}

		#endregion
	}
}