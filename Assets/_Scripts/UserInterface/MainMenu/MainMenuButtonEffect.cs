using DG.Tweening;
using Shoelace.Audio.XuulSound;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Horror.UserInterface.MainMenu
{
	public class MainMenuButtonEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		[Header("Animation Settings")]
		[SerializeField] private float strikethroughDuration = 0.5f;

		[SerializeField] private float randomSkewRange = 5f;
		[SerializeField] private Image strikethrough;

		[SerializeField] private SoundConfig scratchOffSound;
		private bool crossedOff = false;

		public void OnPointerEnter(PointerEventData eventData)
		{
			StrikeOffList();
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			ResetStrike();
		}


		public void StrikeOffList()
		{
			strikethrough.gameObject.SetActive(true);

			if (scratchOffSound != null)
			{
				AudioManager.Instance.PlayOneShot(scratchOffSound);
			}

			float randomOffset = Random.Range(-randomSkewRange, randomSkewRange);
			strikethrough.transform.rotation = Quaternion.Euler(0, 0, randomOffset);

			DOTween.To(() => strikethrough.fillAmount, x => strikethrough.fillAmount = x, 1f, strikethroughDuration)
				.SetEase(Ease.OutQuad);

			crossedOff = true;
		}

		private void ResetStrike()
		{
			strikethrough.fillAmount = 0;
			strikethrough.gameObject.SetActive(false);
			crossedOff = false;
		}
	}
}