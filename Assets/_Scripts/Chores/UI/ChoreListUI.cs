using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Horror.InputSystem;
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

		[Header("Auto-close Settings")]
		[SerializeField] private float autoCloseDelay = 2f;

		private bool isInitializing = false;
		private RectTransform rectTransform;
		private bool isVisible;
		private Tween currentTween;
		private Coroutine autoCloseCoroutine;

		private Dictionary<string, ChoreListEntry> choreEntries;
		
		private void Awake()
		{
			rectTransform = GetComponent<RectTransform>();
			rectTransform.anchoredPosition = hiddenPosition;
			choreEntries = new Dictionary<string, ChoreListEntry>();
			isVisible = false;
		}

		private void Start()
		{
			if (choreManager != null && choreManager.CurrentDayPlan != null)
			{
				HandleDayPlanChanged(choreManager.CurrentDayPlan);
			}
		}

		private void OnEnable()
		{
			SubscribeToEvents();
		}

		private void OnDisable()
		{
			UnsubscribeFromEvents();
		}
		
		private void CreateChoreEntry(ChoreDataSO chore)
		{
			if (chore == null || choreEntries.ContainsKey(chore.ID))
				return;

			ChoreListEntry entry = Instantiate(listEntryPrefab, listEntryContainer);
			entry.Initialize(chore);


			if (choreManager != null)
			{
				ChoreState state = choreManager.GetChoreState(chore);
				entry.SetState(state);
			}

			choreEntries[chore.ID] = entry;
			if (!isInitializing)
			{
				ShowTemporarily();
			}
		}

		#region Pop up

		private void ToggleList() //TODO
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
			if (choreEntries.TryGetValue(choreId, out ChoreListEntry entry))
			{
				entry.SetState(ChoreState.Completed);
				ShowTemporarily();
			}
		}

		private void HandleChoreUnhidden(string choreId)
		{
			if (choreEntries.ContainsKey(choreId)) return;

			ChoreDataSO chore = choreManager.GetChoreById(choreId);
			if (chore != null)
			{
				CreateChoreEntry(chore);
			}
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