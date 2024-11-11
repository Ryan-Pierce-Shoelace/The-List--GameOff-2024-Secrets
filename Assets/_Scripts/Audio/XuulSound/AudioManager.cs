using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using Shoelace.Audio.XuulSound;

namespace Shoelace.Audio
{
	public class AudioManager : MonoBehaviour
	{
		public static AudioManager Instance { get; private set; }

		private void Awake()
		{
			if (Instance != null)
			{
				Debug.LogError("More than one audio manager in the scene");
			}

			Instance = this;
		}


		public void PlayOneShot(SoundConfig config, Vector3 position = default)
		{
			RuntimeManager.PlayOneShot(config.EventRef, position);
		}
	}
}