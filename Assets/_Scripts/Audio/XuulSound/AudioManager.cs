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
		public VolumeSettings VolumeSettings => volumeSettings;

		[Header("Initial Setup")]
		[SerializeField] private SoundConfig startingMusic;

		[SerializeField] private bool playMusicOnStart = true;

		private Bus masterBus;
		private Bus musicBus;
		private Bus sfxBus;
		private Bus ambienceBus;

		[Header("Fade Settings")]
		[SerializeField] private float defaultFadeTime = 2f;

		private Dictionary<string, ISoundPlayer> activeSounds;
		private HashSet<SoundEmitter> activeEmitters;
		private MusicSystem musicSystem;
		private VolumeSettings volumeSettings;

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

			volumeSettings = new VolumeSettings();
			volumeSettings.OnVolumeChanged += HandleVolumeChanged;
			volumeSettings.LoadSettings();

			musicSystem = new MusicSystem();

			masterBus = RuntimeManager.GetBus("bus:/");
			musicBus = RuntimeManager.GetBus("bus:/Music");
			sfxBus = RuntimeManager.GetBus("bus:/SFX");
			ambienceBus = RuntimeManager.GetBus("bus:/Ambience");
		}

		#endregion

		#region Volume Controls

		private void HandleVolumeChanged(VolumeSettings settings)
		{
			UpdateAllVolumes();
		}

		private void UpdateAllVolumes()
		{
			masterBus.setVolume(volumeSettings.Master);
			musicBus.setVolume(volumeSettings.Music);
			sfxBus.setVolume(volumeSettings.SFX);

			float effectiveVolume = volumeSettings.Master * volumeSettings.SFX;
			foreach (ISoundPlayer sound in activeSounds.Values)
			{
				sound.SetVolume(effectiveVolume);
			}

			foreach (SoundEmitter emitter in activeEmitters)
			{
				emitter.SetVolume(effectiveVolume);
			}

			if (musicSystem != null)
			{
				musicSystem.Volume = volumeSettings.Master * volumeSettings.Music;
			}
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

			float effectiveVolume = volumeSettings.Master * volumeSettings.SFX;
			player.SetVolume(effectiveVolume);

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
			if (emitter != null && activeEmitters.Add(emitter))
			{
				float effectiveVolume = volumeSettings.Master * volumeSettings.SFX;
				emitter.SetVolume(effectiveVolume);
			}
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

			if (volumeSettings != null)
			{
				volumeSettings.OnVolumeChanged -= HandleVolumeChanged;
			}
		}

		#endregion
	}
}