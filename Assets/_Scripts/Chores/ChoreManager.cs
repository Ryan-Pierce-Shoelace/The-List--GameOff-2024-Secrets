using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Horror.Chores
{
	public class ChoreManager : MonoBehaviour
	{
		[SerializeField] private DayPlan currentDayPlan;
		private readonly Dictionary<string, ChoreState> choreStates = new Dictionary<string, ChoreState>();

		public event Action<Dictionary<string, ChoreState>> OnChoreStateChanged;

		private void Awake()
		{
			SubscribeToEvents();
			if (currentDayPlan != null)
			{
				LoadDayPlan(currentDayPlan);
			}
		}

		private void SubscribeToEvents()
		{
			ChoreEvents.OnChoreCompleted += HandleChoreCompleted;
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
			foreach (ChoreDataSO chore in currentDayPlan.Chores)
			{
				bool hasUncompletedRequirements = HasUncompletedRequirements(chore);
				choreStates[chore.ID] = hasUncompletedRequirements ? ChoreState.Locked : ChoreState.Available;
			}

			OnChoreStateChanged?.Invoke(choreStates);
		}

		private void HandleChoreCompleted(string choreId)
		{
			if (!choreStates.ContainsKey(choreId)) return;

			choreStates[choreId] = ChoreState.Completed;
			UpdateChoreStates();
			OnChoreStateChanged?.Invoke(choreStates);
		}

		private void UpdateChoreStates()
		{
			foreach (ChoreDataSO chore in currentDayPlan.Chores)
			{
				if (choreStates[chore.ID] == ChoreState.Completed) continue;

				choreStates[chore.ID] = HasUncompletedRequirements(chore) ? ChoreState.Locked : ChoreState.Available;
			}
		}


		private bool HasUncompletedRequirements(ChoreDataSO chore)
		{
			return chore.RequiredChoreIds.Any(id =>
				!choreStates.ContainsKey(id) ||
				choreStates[id] != ChoreState.Completed);
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