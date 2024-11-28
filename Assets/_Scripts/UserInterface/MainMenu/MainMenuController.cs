using DG.Tweening;
using Shoelace.Audio.XuulSound;
using UI.Thoughts;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Horror.UserInterface.MainMenu
{
	public class MainMenuController : MonoBehaviour
	{
		[Header("Scene Settings")]
		[SerializeField] private SceneField levelOne;
		[SerializeField] private SoundConfig gameplayMusic;

		[Header("UI References")]
		[SerializeField] private GameObject settingsPanel;
		[SerializeField] private GameObject shadeOverlay;
		[SerializeField] private GameObject thoughtsPanel;

		[Header("Thoughts")]
		[SerializeField] private DynamicThoughtSO thoughts;

		[SerializeField] private float interval = 5f;
		private float nextCheckTime;
		private bool isLoading = false;

		private void Start()
		{
			//if (fadePanel != null)
			//{
			//	fadePanel.gameObject.SetActive(true);
			//	fadePanel.color = new Color(0, 0, 0, 0);
			//}

			if (settingsPanel != null)
			{
				settingsPanel.SetActive(false);
			}
		}

		private void Update()
		{
			if (!thoughts || isLoading) return;

			nextCheckTime -= Time.deltaTime;

			if (!(nextCheckTime <= 0f)) return;

			thoughts.PlayThought();
			nextCheckTime = interval;
		}


		public void BeginGame()
		{
			DisableAllButtons();
			isLoading = true;
			thoughtsPanel.SetActive(false);
			//Sequence transitionSequence = DOTween.Sequence();


			FadeTransition.Instance.ChangeDay(levelOne, "Day 1");
			AudioManager.Instance.PlayMusic(gameplayMusic);

			//if (fadePanel != null)
			//{
			//	fadePanel.DOFade(1f, fadeOutDuration)
			//		.OnComplete((() => SceneManager.LoadScene(gameSceneName)));

			//	if (AudioManager.Instance != null)
			//	{
			//		float currentVolume = AudioManager.Instance.MasterVolume;
			//		transitionSequence.Join(
			//			DOTween.To(
			//				() => AudioManager.Instance.MasterVolume,
			//				value => AudioManager.Instance.MasterVolume = value,
			//				0f,
			//				fadeOutDuration
			//			).SetEase(Ease.InOutQuad)
			//		);
			//	}
			//}
			//else
			//{
			//	SceneManager.LoadScene(gameSceneName);
			//}
		}


		private void DisableAllButtons()
		{
			Button[] allButtons = GetComponentsInChildren<Button>(true);
			foreach (Button button in allButtons)
			{
				button.interactable = false;
			}
		}


		public void OnQuitClicked()
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
		}
	}
}