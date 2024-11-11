using System;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Shoelace.Audio.XuulSound
{
	[CreateAssetMenu(fileName = "New Sound Config", menuName = "Audio/Sound Config")]
	public class SoundConfig : ScriptableObject
	{
		[SerializeField] private EventReference eventReference;
		[SerializeField] private bool is3D;
		[SerializeField] private float defaultVolume = 1f;
		[SerializeField] private bool useParameters;

		[SerializeField, ShowIf("useParameters")]
		private List<ParameterConfig> parameters;

		public EventReference EventRef => eventReference;
		public bool Is3D => is3D;
		public float DefaultVolume => defaultVolume;

		[Serializable]
		public struct ParameterConfig
		{
			public string Name;
			public PARAMETER_ID ID;
			public float DefaultValue;
		}
	}
}