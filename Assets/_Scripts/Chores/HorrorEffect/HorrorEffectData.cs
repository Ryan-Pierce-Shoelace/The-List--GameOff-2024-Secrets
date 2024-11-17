using System;
using UnityEngine;

namespace Horror.Chores.HorrorEffect
{
	[System.Serializable]
	public class HorrorEffectData
	{
		public string OverrideText = "Drink to Forget";
		public float Duration;
		[HideInInspector] public float LikelyHood;
		[HideInInspector] public Color TextColor;
		[HideInInspector] public float ShakeAmount;
		[HideInInspector] public int ShakeVibraton;

		public HorrorEffectData(string text, float duration, float likelyHood = 0.5f)
		{
			OverrideText = text;
			Duration = duration;
			LikelyHood = Math.Clamp(likelyHood, 0, 1);
			
			TextColor = Color.red;
			ShakeAmount = 5f;
			ShakeVibraton = 10;
		}
	}
}