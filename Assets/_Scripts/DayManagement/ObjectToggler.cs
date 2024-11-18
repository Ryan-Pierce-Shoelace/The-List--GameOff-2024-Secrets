using System.Collections;
using Shoelace.Audio.XuulSound;
using UnityEngine;

namespace Horror.DayManagement
{
	public class ObjectToggler : MonoBehaviour
	{
		[SerializeField] private GameObject targetObject;
		[SerializeField] private float enabledDuration = 1f;
		[SerializeField] private float disabledDuration = 0.5f;
		[SerializeField] private bool startEnabled = true;

		[Header("Audio")]
		[SerializeField] private SoundConfig enableSound;

		[SerializeField] private SoundConfig disableSound;

		private ISoundPlayer enableSoundPlayer;
		private ISoundPlayer disableSoundPlayer;

		private void Start()
		{
			if (targetObject == null)
			{
				targetObject = gameObject;
			}

			if (enableSound)
			{
				enableSoundPlayer = new SimpleSoundPlayer(enableSound);
			}

			if (disableSound)
			{
				disableSoundPlayer = new SimpleSoundPlayer(disableSound);
			}

			targetObject.SetActive(startEnabled);
			StartCoroutine(ToggleRoutine());
		}

		private IEnumerator ToggleRoutine()
		{
			while (true)
			{
				yield return new WaitForSeconds(targetObject.activeSelf ? enabledDuration : disabledDuration);
				bool newState = !targetObject.activeSelf;
				targetObject.SetActive(newState);

				if (newState)
				{
					enableSoundPlayer?.Play();
				}
				else
				{
					disableSoundPlayer?.Play();
				}
			}
		}

		public void RestartFlicker()
		{
			StopAllCoroutines();
			StartCoroutine(ToggleRoutine());
		}

		public void StopFlicker()
		{
			StopAllCoroutines();
		}

		public void SetActive(bool state)
		{
			targetObject.SetActive(state);
		}
	}
}