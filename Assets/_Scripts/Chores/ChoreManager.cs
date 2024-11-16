using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Horror.Chores
{
	public class ChoreManager : MonoBehaviour
	{
		[FormerlySerializedAs("currentDayPlan")] [SerializeField]
		public DayPlan CurrentDayPlan;

		private Dictionary<ChoreDataSO, ChoreState> choreStates;
		private Dictionary<string, ChoreDataSO> choreIdLookup;

		private void Awake()
		{
			choreStates = new Dictionary<ChoreDataSO, ChoreState>();
			choreIdLookup = new Dictionary<string, ChoreDataSO>();

			SubscribeToEvents();
			if (CurrentDayPlan != null)
			{
				SetDayPlan(CurrentDayPlan);
			}
		}

		private void SubscribeToEvents()
		{
			ChoreEvents.OnChoreCompleted += HandleChoreCompleted;
			ChoreEvents.OnChoreAdvanced += ProgressChore;
			ChoreEvents.OnDayReset += ResetChores;
			ChoreEvents.OnChoreUnhidden += HandleChoreUnhidden; 
		}

		private void UnsubscribeToEvents()
		{
			ChoreEvents.OnChoreCompleted -= HandleChoreCompleted;
			ChoreEvents.OnChoreAdvanced -= ProgressChore;
			ChoreEvents.OnDayReset -= ResetChores;
			ChoreEvents.OnChoreUnhidden -= HandleChoreUnhidden; 
		}


		public void SetDayPlan(DayPlan newPlan)
		{
			if (newPlan == null) throw new ArgumentNullException(nameof(newPlan));

			CurrentDayPlan = newPlan;
			InitializeChores();
			ChoreEvents.ChangeDayPlan(CurrentDayPlan);
		}


		private void InitializeChores()
		{
			choreStates.Clear();
			choreIdLookup.Clear();
			foreach (ChoreDataSO chore in CurrentDayPlan.Chores)
			{
				chore.Reset();
				bool hasUncompletedRequirements = HasUncompletedRequirements(chore);
				choreStates[chore] = hasUncompletedRequirements ? ChoreState.Locked : ChoreState.Available;
				choreIdLookup[chore.ID] = chore;
			}
		}


		#region State Handlers
		private void HandleChoreCompleted(string choreId)
		{
			if (!choreIdLookup.TryGetValue(choreId, out ChoreDataSO chore)) return;

			choreStates[chore] = ChoreState.Completed;
			UpdateChoreStates();
		}
		private void HandleChoreUnhidden(string choreId) 
		{
			if (!choreIdLookup.TryGetValue(choreId, out ChoreDataSO chore)) return;
			if (choreStates[chore] != ChoreState.Hidden) return;

			bool hasUncompletedRequirements = HasUncompletedRequirements(chore);
			choreStates[chore] = hasUncompletedRequirements ? ChoreState.Locked : ChoreState.Available;
			UpdateChoreStates();
		}
		private void ProgressChore(string choreId)
		{
			if (!choreIdLookup.TryGetValue(choreId, out ChoreDataSO chore)) return;

			if (choreStates[chore] != ChoreState.Available) return;

			chore.Increment();
		}

		#endregion

	

		
		
		

		private void UpdateChoreStates()
		{
			foreach (ChoreDataSO chore in CurrentDayPlan.Chores)
			{
				if (choreStates[chore] == ChoreState.Completed) continue;

				choreStates[chore] = HasUncompletedRequirements(chore) ? ChoreState.Locked : ChoreState.Available;
			}
		}

		private bool HasUncompletedRequirements(ChoreDataSO chore)
		{
			if (chore.RequiredChores == null || chore.RequiredChores.Count == 0)
			{
				return false;
			}

			return chore.RequiredChores.Any(requiredChore => 
				requiredChore == null || 
				!choreIdLookup.ContainsKey(requiredChore.ID) || 
				choreStates[choreIdLookup[requiredChore.ID]] != ChoreState.Completed);
		}

		private void ResetChores() => InitializeChores();

		public ChoreState GetChoreState(ChoreDataSO chore)
		{
			if (chore == null)
			{
				return ChoreState.Locked;
			}

			if (choreStates != null && choreStates.TryGetValue(chore, out ChoreState state))
			{
				return state;
			}


			return ChoreState.Locked;
		}

		public ChoreDataSO GetChoreById(string choreId)
		{
			if (choreIdLookup != null && choreIdLookup.TryGetValue(choreId, out ChoreDataSO chore))
			{
				return chore;
			}

			return null;
		}

		private void OnDestroy()
		{
			UnsubscribeToEvents();
		}
	}
}