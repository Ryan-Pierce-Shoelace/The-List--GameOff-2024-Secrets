using DG.Tweening;
using UI.Thoughts;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Horror.UserInterface.MainMenu
{
	public class MainMenuController : MonoBehaviour
	{
		[Header("Scene Settings")]
		[SerializeField] private string gameSceneName = "Game";

		[SerializeField] private float fadeOutDuration = 1f;

		[Header("UI References")]
		[SerializeField] private Image fadePanel;

		[SerializeField] private GameObject settingsPanel;
		[SerializeField] private GameObject shadeOverlay;

		[Header("Thoughts")]
		[SerializeField] private DynamicThoughtSO thoughts;

		[SerializeField] private float interval = 5f;
		private float nextCheckTime;
		private bool isLoading = false;

		private void Start()
		{
			if (fadePanel != null)
			{
				fadePanel.gameObject.SetActive(true);
				fadePanel.color = new Color(0, 0, 0, 0);
			}

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

			if (fadePanel != null)
			{
				fadePanel.DOFade(1f, fadeOutDuration)
					.OnComplete((() => SceneManager.LoadScene(gameSceneName)));
			}
			else
			{
				SceneManager.LoadScene(gameSceneName);
			}
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