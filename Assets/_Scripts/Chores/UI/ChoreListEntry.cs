using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Horror.Chores.UI
{
	public class ChoreListEntry : MonoBehaviour
	{
		public float StrikethroughDuration => strikethroughDuration;
		public bool IsTutorial => choreData.IsTutorialChore;

		[Header("References")]
		[SerializeField] private TextMeshProUGUI choreName;

		[SerializeField] private Image strikethrough;
		[SerializeField] private TextMeshProUGUI progressCounter;

		[Header("Visual Settings")]
		[SerializeField] private Color unavailableColor = new Color(0.7f, 0.7f, 0.7f);

		[SerializeField] private Color availableColor = Color.black;

		[Header("Animation Settings")]
		[SerializeField] private float strikethroughDuration = 0.5f;

		[SerializeField] private float randomSkewRange = 5f;


		private ChoreDataSO choreData;
		private ChoreState currentState;

		private void Awake()
		{
			if (strikethrough == null) return;

			strikethrough.fillAmount = 0;
			strikethrough.transform.localRotation = Quaternion.identity;
		}

		private void OnEnable()
		{
			ChoreEvents.OnChoreAdvanced += HandleChoreAdvanced;
		}

		private void OnDisable()
		{
			ChoreEvents.OnChoreAdvanced -= HandleChoreAdvanced;
		}

		public void Initialize(ChoreDataSO chore)
		{
			choreData = chore;
			choreName.text = chore.ChoreName;
			UpdateProgressCounter();
			SetState(ChoreState.Locked);
		}

		public void SetState(ChoreState newState)
		{
			currentState = newState;
			UpdateVisuals();
		}

		public void UpdateProgressCounter()
		{
			if (choreData.RequiredCount > 1)
			{
				progressCounter.gameObject.SetActive(currentState == ChoreState.Available);
				progressCounter.text = $"{choreData.CurrentCount}/{choreData.RequiredCount}";
			}
			else
			{
				progressCounter.gameObject.SetActive(false);
			}
		}

		private void UpdateVisuals()
		{
			switch (currentState)
			{
				case ChoreState.Locked:
					HandleLockedState();
					break;
				case ChoreState.Available:
					HandleAvailableState();
					break;
				case ChoreState.Completed:
					HandleCompletedState();
					break;
			}
		}

		private void HandleChoreAdvanced(string choreId)
		{
			if (choreData != null && choreData.ID == choreId)
			{
				UpdateProgressCounter();
			}
		}

		private void HandleLockedState()
		{
			choreName.color = unavailableColor;
			progressCounter.gameObject.SetActive(false);
			strikethrough.fillAmount = 0;
		}

		private void HandleAvailableState()
		{
			choreName.color = availableColor;
			UpdateProgressCounter();
			strikethrough.fillAmount = 0;
		}


		private void HandleCompletedState()
		{
			choreName.color = unavailableColor;
			progressCounter.gameObject.SetActive(false);

			float randomOffset = Random.Range(-randomSkewRange, randomSkewRange);

			Sequence completionSequence = DOTween.Sequence();

			completionSequence.Append(
				strikethrough.transform.DORotate(new Vector3(0, 0, randomOffset), strikethroughDuration * 0.3f)
			);

			completionSequence.Append(
				DOTween.To(() => strikethrough.fillAmount, x => strikethrough.fillAmount = x, 1f, strikethroughDuration)
					.SetEase(Ease.OutQuad)
			);
		}
	}
}