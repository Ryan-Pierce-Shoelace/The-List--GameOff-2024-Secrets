using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Horror.InputSystem;
using Shoelace.Audio.XuulSound;
using UnityEngine;

namespace Horror.Chores.UI
{
	public class ChoreListUI : MonoBehaviour
	{
		[Header("References")]
		[SerializeField] private ChoreListEntry listEntryPrefab;

		[SerializeField] private Transform listEntryContainer;
		[SerializeField] private InputReader input;
		[SerializeField] private ChoreManager choreManager;

		[Header("Animation Settings")]
		[SerializeField] private float animationDuration = 0.5f;

		//TODO set up math to auto set this up	
		[SerializeField] private Vector2 hiddenPosition = new Vector2(0, -100);
		[SerializeField] private Vector2 visiblePosition = new Vector2(0, 100);
		[SerializeField] private float autoCloseDelay = 2f;

		[Header("Sounds")]
		[SerializeField] private SoundConfig writingSound;

		[SerializeField] private SoundConfig penStrokeSound;

		private ISoundPlayer writingPlayer;
		private ISoundPlayer penStrokePlayer; //TODO ask if one shot is right or if we hsould have a player?


		private bool isInitializing = false;
		private RectTransform rectTransform;
		private bool isVisible;
		private Tween currentTween;
		private Coroutine autoCloseCoroutine;

		private Dictionary<string, ChoreListEntry> choreEntries;
		private Dictionary<string, (ChoreListEntry Entry, CanvasGroup Group)> tutorialEntries;

		private void Awake()
		{
			rectTransform = GetComponent<RectTransform>();
			rectTransform.anchoredPosition = visiblePosition;
			choreEntries = new Dictionary<string, ChoreListEntry>();
			tutorialEntries = new Dictionary<string, (ChoreListEntry, CanvasGroup)>();
			isVisible = true;

			if (choreManager != null) return;

			choreManager = FindObjectOfType<ChoreManager>();
			if (choreManager == null)
			{
				Debug.LogError("No ChoreManager found in scene! ChoreListUI requires a ChoreManager to function.", this);
			}
		}

		private void Start()
		{
			if (choreManager != null && choreManager.CurrentDayPlan != null)
			{
				HandleDayPlanChanged(choreManager.CurrentDayPlan);
			}

			penStrokePlayer ??= AudioManager.Instance.CreateSound(penStrokeSound);
		}

		private void OnEnable()
		{
			SubscribeToEvents();
		}

		private void OnDisable()
		{
			UnsubscribeFromEvents();
			StopAllCoroutines();
		}


		private void CreateChoreEntry(ChoreDataSO chore)
		{
			if (!chore || choreEntries.ContainsKey(chore.ID))
				return;

			ChoreListEntry entry = Instantiate(listEntryPrefab, listEntryContainer);
			entry.Initialize(chore);

			if (chore.IsTutorialChore)
			{
				var canvasGroup = entry.gameObject.AddComponent<CanvasGroup>();
				tutorialEntries[chore.ID] = (entry, canvasGroup);
			}

			if (choreManager)
			{
				ChoreState state = choreManager.GetChoreState(chore);
				entry.SetState(state);
			}

			choreEntries[chore.ID] = entry;
			if (!isInitializing)
			{
				ShowTemporarily();
				AudioManager.Instance.PlayOneShot(writingSound);
			}
		}

		#region Pop up

		private void ToggleList()
		{
			if (isVisible)
			{
				Close();
			}
			else
			{
				Open();
			}
		}

		private void ShowTemporarily()
		{
			Open(autoClose: true);
		}

		private void Open(bool autoClose = false)
		{
			if (isVisible) return;


			if (autoCloseCoroutine != null)
			{
				StopCoroutine(autoCloseCoroutine);
				autoCloseCoroutine = null;
			}


			currentTween?.Kill();


			currentTween = rectTransform.DOAnchorPos(visiblePosition, animationDuration)
				.SetEase(Ease.OutBack);

			isVisible = true;

			if (autoClose)
			{
				autoCloseCoroutine = StartCoroutine(AutoCloseCoroutine());
			}
		}

		private void Close()
		{
			if (!isVisible) return;


			if (autoCloseCoroutine != null)
			{
				StopCoroutine(autoCloseCoroutine);
				autoCloseCoroutine = null;
			}

			currentTween?.Kill();

			currentTween = rectTransform.DOAnchorPos(hiddenPosition, animationDuration)
				.SetEase(Ease.InBack);

			isVisible = false;
		}

		private IEnumerator AutoCloseCoroutine()
		{
			yield return new WaitForSeconds(autoCloseDelay);
			Close();
		}

		#endregion


		private void HandleDayPlanChanged(DayPlan newPlan)
		{
			if (newPlan == null)
			{
				Debug.LogWarning("Received null DayPlan");
				return;
			}

			Debug.Log($"Creating UI for day plan with {newPlan.Chores.Count} chores");
			ClearList();

			isInitializing = true;

			foreach (ChoreDataSO chore in newPlan.Chores)
			{
				if (!chore.StartsHidden)
				{
					CreateChoreEntry(chore);
				}
			}

			isInitializing = false;
		}


		private void HandleChoreAdvanced(string choreId)
		{
			if (choreEntries.TryGetValue(choreId, out ChoreListEntry entry))
			{
				entry.UpdateProgressCounter();
			}
		}

		private void HandleChoreCompleted(string choreId)
		{
			if (!choreEntries.TryGetValue(choreId, out ChoreListEntry entry)) return;

			ShowTemporarily();

			if (isVisible)
			{
				entry.SetState(ChoreState.Completed);
				AudioManager.Instance.PlayOneShot(writingSound);

				if (entry.IsTutorial && tutorialEntries.TryGetValue(choreId, out (ChoreListEntry Entry, CanvasGroup Group) tutorialEntry))
				{
					StartCoroutine(RemoveTutorialChore(choreId, tutorialEntry.Entry, tutorialEntry.Group));
				}
			}
			else
			{
				StartCoroutine(DelayedChoreCompletion(entry));
			}
		}

		private IEnumerator DelayedChoreCompletion(ChoreListEntry entry)
		{
			yield return new WaitForSeconds(animationDuration + .1f);
			entry.SetState(ChoreState.Completed);
			penStrokePlayer?.Play();
		}


		private void HandleChoreUnhidden(string choreId)
		{
			if (choreEntries.ContainsKey(choreId)) return;

			ChoreDataSO chore = choreManager.GetChoreById(choreId);
			if (chore)
			{
				CreateChoreEntry(chore);
			}
		}

		private IEnumerator RemoveTutorialChore(string choreId, ChoreListEntry entry, CanvasGroup group)
		{
			yield return new WaitForSeconds(entry.StrikethroughDuration + 0.5f);

			DOTween.Sequence()
				.Append(entry.transform.DOScale(0, 0.3f))
				.Join(group.DOFade(0, 0.3f))
				.OnComplete(() =>
				{
					choreEntries.Remove(choreId);
					tutorialEntries.Remove(choreId);
					Destroy(entry.gameObject);
				});
		}

		private void ClearList()
		{
			foreach (ChoreListEntry entry in choreEntries.Values)
			{
				if (entry != null)
				{
					Destroy(entry.gameObject);
				}
			}

			choreEntries.Clear();
			Debug.Log("Cleared chore list");
		}


		private void SubscribeToEvents()
		{
			ChoreEvents.OnDayPlanChanged += HandleDayPlanChanged;
			ChoreEvents.OnChoreCompleted += HandleChoreCompleted;
			ChoreEvents.OnChoreAdvanced += HandleChoreAdvanced;
			ChoreEvents.OnChoreUnhidden += HandleChoreUnhidden;
			ChoreEvents.OnDayReset += ClearList;

			if (input != null)
			{
				input.ToggleListEvent += ToggleList;
			}
		}

		private void UnsubscribeFromEvents()
		{
			ChoreEvents.OnDayPlanChanged -= HandleDayPlanChanged;
			ChoreEvents.OnChoreCompleted -= HandleChoreCompleted;
			ChoreEvents.OnChoreAdvanced -= HandleChoreAdvanced;
			ChoreEvents.OnChoreUnhidden -= HandleChoreUnhidden;
			ChoreEvents.OnDayReset -= ClearList;

			if (input != null)
			{
				input.ToggleListEvent -= ToggleList;
			}
		}
	}
}