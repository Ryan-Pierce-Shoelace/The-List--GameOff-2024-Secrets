using UnityEngine;

namespace Horror.UserInterface
{
	public class Credits : MonoBehaviour
	{
		[SerializeField] private GameObject creditsPanel;

		public void OpenCredits()
		{
			if (creditsPanel != null)
			{
				creditsPanel.SetActive(true);
			}
		}

		public void CloseCredits()
		{
			if (creditsPanel != null)
			{
				creditsPanel.SetActive(false);
			}
		}
	}
}