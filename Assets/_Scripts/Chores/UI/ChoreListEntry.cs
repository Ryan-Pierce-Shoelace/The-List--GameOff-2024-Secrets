using DG.Tweening;
using Horror.Chores.HorrorEffect;
using Shoelace.Audio.XuulSound;
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
		
		private string originalText;
		private Color originalColor;
		private Vector3 orignalPosition;
		private Sequence horrorSequence;
		private bool crossedOff;

		private void Awake()
		{
			if (strikethrough == null) return;

			strikethrough.fillAmount = 0;
			strikethrough.transform.localRotation = Quaternion.identity;
			crossedOff = false;
		}

		private void OnEnable()
		{
			ChoreEvents.OnChoreAdvanced += HandleChoreAdvanced;
			ChoreEvents.OnChoreHorrorEffect += HandleHorrorEffect;
		}

		private void OnDisable()
		{
			ChoreEvents.OnChoreAdvanced -= HandleChoreAdvanced;
			ChoreEvents.OnChoreHorrorEffect -= HandleHorrorEffect;
			horrorSequence?.Kill();
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
					if(!crossedOff)
					{
                        HandleCompletedState();
                    }
					break;
			}
		}

		private void HandleChoreAdvanced(string choreId)
		{
			if (choreData && choreData.ID == choreId)
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
			
            crossedOff = true;
        }
		
		private void HandleHorrorEffect(string choreId, HorrorEffectData effectData)
		{
			if (!choreData || choreData.ID != choreId) return;
			
			if (string.IsNullOrEmpty(originalText))
			{
				originalText = choreName.text;
				originalColor = choreName.color;
			}

			orignalPosition = choreName.transform.localPosition;
			horrorSequence?.Kill();
			
			horrorSequence = DOTween.Sequence();


			choreName.color = effectData.TextColor;
			if (!string.IsNullOrEmpty(effectData.OverrideText))
			{
				choreName.text = effectData.OverrideText;
			}
			
			horrorSequence.Append(
				choreName.transform.DOShakePosition(
					effectData.Duration, 
					strength: effectData.ShakeAmount,
					vibrato: effectData.ShakeVibraton,
					randomness: 90,
					snapping: false,
					fadeOut: true)
			);


			horrorSequence.OnComplete(() => {
				choreName.text = originalText;
				choreName.color = originalColor;
				choreName.transform.localPosition = orignalPosition;
				horrorSequence = null;
			});
		}
	}
}