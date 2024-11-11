#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Shoelace.Audio.XuulSound.Editor
{
	[CustomEditor(typeof(SoundConfig))]
	public class SoundConfigEditor : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			SoundConfig soundConfig = (SoundConfig)target;

			if (GUILayout.Button("Load Parameters"))
			{
				soundConfig.LoadParameterDescriptions();
				EditorUtility.SetDirty(soundConfig);
			}
		}
	}
}
#endif