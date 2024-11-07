using System.Collections;
using TMPro;
using UnityEngine;

namespace Text_Display
{
	public class TextDisplayManager : MonoBehaviour
	{
		[Header("UI References")]
		[SerializeField] private TextMeshProUGUI thoughtText;

		[Header("Thought Display Settings")]
		[SerializeField] private float displayTime = 3f;

		[SerializeField] private float fadeOutDuration = 1f;

		private Coroutine currentDisplayCoroutine;

		private void Start()
		{
			if (thoughtText != null)
			{
				thoughtText.alpha = 0f;
			}
		}

		private void OnEnable()
		{
			ThoughtEvent.OnThoughtTriggered += DisplayThought;
		}


		private void OnDisable()
		{
			ThoughtEvent.OnThoughtTriggered -= DisplayThought;
		}


		private void DisplayThought(string thought)
		{
			if (thoughtText == null) return;

			if (currentDisplayCoroutine != null)
			{
				StopCoroutine(currentDisplayCoroutine);
			}

			currentDisplayCoroutine = StartCoroutine(DisplayThoughtCoroutine(thought));
		}

		private IEnumerator DisplayThoughtCoroutine(string thought)
		{
			thoughtText.text = thought;
			thoughtText.alpha = 1f;


			yield return new WaitForSeconds(displayTime);


			float elapsedTime = 0f;
			while (elapsedTime < fadeOutDuration)
			{
				elapsedTime += Time.deltaTime;
				thoughtText.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeOutDuration);
				yield return null;
			}


			thoughtText.alpha = 0f;
		}
	}
}