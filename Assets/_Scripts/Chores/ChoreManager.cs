using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Horror.Chores
{
	public class ChoreManager : MonoBehaviour
	{
		[SerializeField] private DayPlan currentDayPlan;
		private Dictionary<ChoreDataSO, ChoreState> choreStates;
		private Dictionary<string, ChoreDataSO> choreIdLookup;
		
		private void Awake()
		{
			choreStates = new Dictionary<ChoreDataSO, ChoreState>();
			choreIdLookup = new Dictionary<string, ChoreDataSO>();
			
			SubscribeToEvents();
			if (currentDayPlan != null)
			{
				LoadDayPlan(currentDayPlan);
			}
		}

		private void SubscribeToEvents()
		{
			ChoreEvents.OnChoreCompleted += HandleChoreCompleted;
			ChoreEvents.OnChoreAdvanced += ProgressChore;
			ChoreEvents.OnDayReset += ResetChores;
			ChoreEvents.OnDayPlanChanged += LoadDayPlan;
		}

		private void LoadDayPlan(DayPlan plan)
		{
			currentDayPlan = plan ?? throw new ArgumentNullException(nameof(plan));
			InitializeChores();
		}

		private void InitializeChores()
		{
			choreStates.Clear();
			choreIdLookup.Clear();
			foreach (ChoreDataSO chore in currentDayPlan.Chores)
			{
				chore.Reset();
				bool hasUncompletedRequirements = HasUncompletedRequirements(chore);
				choreStates[chore] = hasUncompletedRequirements ? ChoreState.Locked : ChoreState.Available;
				choreIdLookup[chore.ID] = chore;
			}
		}

		private void HandleChoreCompleted(string choreId)
		{
			if (!choreIdLookup.TryGetValue(choreId, out ChoreDataSO chore)) return;

			choreStates[chore] = ChoreState.Completed;
			Debug.Log($"Chore {chore.ID} is comepleted");
			UpdateChoreStates();
		}

		private void ProgressChore(string choreId)
		{
			if (!choreIdLookup.ContainsKey(choreId)) return;
            
			ChoreDataSO chore = choreIdLookup[choreId];
			if (choreStates[chore] == ChoreState.Available)
			{
				chore.Increment();
			}
		}

		private void UpdateChoreStates()
		{
			foreach (ChoreDataSO chore in currentDayPlan.Chores)
			{
				if (choreStates[chore] == ChoreState.Completed) continue;

				choreStates[chore] = HasUncompletedRequirements(chore) ? ChoreState.Locked : ChoreState.Available;
			}
		}

		private bool HasUncompletedRequirements(ChoreDataSO chore)
		{
			return chore.RequiredChoreIds.Any(id =>
				!choreIdLookup.ContainsKey(id) ||
				choreStates[choreIdLookup[id]] != ChoreState.Completed);
		}

		private void ResetChores() => InitializeChores();
		

		private void OnDestroy()
		{
			ChoreEvents.OnChoreCompleted -= HandleChoreCompleted;
			ChoreEvents.OnDayReset -= ResetChores;
			ChoreEvents.OnDayPlanChanged -= LoadDayPlan;
		}
	}
}