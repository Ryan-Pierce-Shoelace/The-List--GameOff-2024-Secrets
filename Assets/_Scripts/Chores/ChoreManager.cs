using System;
using System.Collections.Generic;
using System.Linq;
using Horror.Chores.HorrorEffect;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Horror.Chores
{
	public class ChoreManager : MonoBehaviour
	{
		public static ChoreManager Instance; 
		public DayPlan CurrentDayPlan => currentDayPlan;

		[Header("Day Plan")]
		[SerializeField] private DayPlan currentDayPlan;
		
		[Header("Horror Effect Settings")]
        [SerializeField] private float startHorrorDelay = 10f;
        [SerializeField] private float interval = 10f;

		private Dictionary<string, ChoreState> choreStates;
		private Dictionary<string, ChoreDataSO> choreIdLookup;


		private float nextCheckTime;
		

		private void Awake()
		{
			choreStates = new Dictionary<string, ChoreState>();
			choreIdLookup = new Dictionary<string, ChoreDataSO>();

			SubscribeToEvents();
			if (CurrentDayPlan != null)
			{
				SetDayPlan(CurrentDayPlan);
			}

			if (Instance == null)
			{
				Instance = this;
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

			currentDayPlan = newPlan;
			InitializeChores();
			ChoreEvents.ChangeDayPlan(CurrentDayPlan);
		}


		private void InitializeChores()
		{
			nextCheckTime = startHorrorDelay;
			choreStates.Clear();
			choreIdLookup.Clear();
			foreach (ChoreDataSO chore in CurrentDayPlan.Chores)
			{
				chore.Reset();
				bool hasUncompletedRequirements = HasUncompletedRequirements(chore);
				choreStates[chore.ID] = hasUncompletedRequirements ? ChoreState.Locked : ChoreState.Available;
				choreIdLookup[chore.ID] = chore;
			}
		}

		private void Update()
		{
			nextCheckTime -= Time.deltaTime;

			if (!(nextCheckTime <= 0f)) return;
			
			CheckHorrorEffects();
			nextCheckTime = interval;
		}

		#region HorrorEffextText

		private bool RollForScaryText(HorrorEffectData effectData)
		{
			return Random.value < effectData.LikelyHood;
		}

		private void CheckHorrorEffects()
		{
			foreach (KeyValuePair<ChoreDataSO, HorrorEffectData> kvp in currentDayPlan.DailyHorrorEffects)
			{
				ChoreDataSO chore = kvp.Key;
				HorrorEffectData data = kvp.Value;
				ChoreState currentState = GetChoreState(chore);
				
				if(currentState is not ChoreState.Available or ChoreState.Completed) continue;
				
				if(RollForScaryText(data))
				{
					ChoreEvents.TriggerHorrorEffect(chore.ID, data);
				}
			}
		}

		#endregion


		#region State Handlers

		private void HandleChoreCompleted(string choreId)
		{
			if (!choreIdLookup.TryGetValue(choreId, out ChoreDataSO chore)) return;

			choreStates[chore.ID] = ChoreState.Completed;

			UpdateChoreStates();
		}

		private void HandleChoreUnhidden(string choreId)
		{
			if (!choreIdLookup.TryGetValue(choreId, out ChoreDataSO chore)) return;
			if (choreStates[chore.ID] != ChoreState.Hidden) return;

			bool hasUncompletedRequirements = HasUncompletedRequirements(chore);
			choreStates[chore.ID] = hasUncompletedRequirements ? ChoreState.Locked : ChoreState.Available;
			UpdateChoreStates();
		}

		private void ProgressChore(string choreId)
		{
			if (!choreIdLookup.TryGetValue(choreId, out ChoreDataSO chore)) return;

			if (choreStates[chore.ID] != ChoreState.Available) return;

			chore.Increment();
		}
        #endregion


        private void UpdateChoreStates()
		{
			bool statesChanged;
			do
			{
				statesChanged = false;
				foreach (ChoreDataSO chore in CurrentDayPlan.Chores)
				{
					if (choreStates[chore.ID] == ChoreState.Completed)
						continue;

					ChoreState oldState = choreStates[chore.ID];
					ChoreState newState = HasUncompletedRequirements(chore) ? ChoreState.Locked : ChoreState.Available;

					if (oldState != newState)
					{
						choreStates[chore.ID] = newState;
						statesChanged = true;
					}
				}
			} while (statesChanged);
		}


		private bool HasUncompletedRequirements(ChoreDataSO chore)
		{
			if (chore.RequiredChores == null || chore.RequiredChores.Count == 0)
			{
				return false;
			}

			return chore.RequiredChores.Any(requiredChore =>
				!requiredChore ||
				!choreIdLookup.ContainsKey(requiredChore.ID) ||
				choreStates[requiredChore.ID] != ChoreState.Completed);
		}

		private void ResetChores() => InitializeChores();

		public ChoreState GetChoreState(ChoreDataSO chore)
		{
			if (!chore || choreStates == null)
			{
				return ChoreState.Locked;
			}

			if (choreStates != null && choreStates.TryGetValue(chore.ID, out ChoreState state))
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