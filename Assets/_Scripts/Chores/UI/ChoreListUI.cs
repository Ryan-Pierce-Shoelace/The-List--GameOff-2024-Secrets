using System;
using System.Collections.Generic;
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
        
		private Dictionary<string, ChoreListEntry> choreEntries;
		private bool isVisible = true; 
		

	

		private void Awake()
		{
			choreEntries = new Dictionary<string, ChoreListEntry>();
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
		}

		private void ToggleList() //TODO
		{
			isVisible = !isVisible;
			Debug.Log($"List visibility toggled: {isVisible}");
		}


		private void HandleDayPlanChanged(DayPlan newPlan)
		{
			if (newPlan == null)
			{
				Debug.LogWarning("Received null DayPlan");
				return;
			}

			Debug.Log($"Creating UI for day plan with {newPlan.Chores.Count} chores");
			ClearList();

			foreach (ChoreDataSO chore in newPlan.Chores)
			{
				CreateChoreEntry(chore);
			}
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
			ChoreEvents.OnDayReset -= ClearList;

			if (input != null)
			{
				input.ToggleListEvent -= ToggleList;
			}
		}
	}
}