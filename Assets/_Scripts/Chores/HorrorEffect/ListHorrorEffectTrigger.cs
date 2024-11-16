using Sirenix.OdinInspector;
using UnityEngine;

namespace Horror.Chores.HorrorEffect
{
	public class ListHorrorEffectTrigger : MonoBehaviour
	{
		[SerializeField] private ChoreDataSO choreData;
		[SerializeField] private string horrorText = "";

		private HorrorEffectData horrorEffect;

		private void Awake()
		{
			horrorEffect = new HorrorEffectData
			{
				OverrideText = horrorText,
				Duration = 3f,
				TextColor = Color.red,
				ShakeAmount = 5f,
				ShakeVibrato = 10
			};
		}

		[Button]
		public void TriggerEffect(float duration = 3f)
		{
			horrorEffect.Duration = duration;

			ChoreEvents.TriggerHorrorEffect(choreData.ID, horrorEffect);
		}
	}
}