using System;

namespace Horror.Chores
{
	public static class ChoreEvents
	{
		public static event Action<string> OnChoreCompleted;
		public static event Action<string> OnChoreAdvanced;
		public static event Action OnDayReset;
		public static event Action<DayPlan> OnDayPlanChanged;
		public static event Action<string> OnChoreUnhidden;  

		public static void CompleteChore(string choreId) => OnChoreCompleted?.Invoke(choreId);
		public static void AdvanceChore(string choreId) => OnChoreAdvanced?.Invoke(choreId);
		public static void ResetDay() => OnDayReset?.Invoke();
		public static void ChangeDayPlan(DayPlan plan) => OnDayPlanChanged?.Invoke(plan);  
		public static void UnhideChore(string choreId) => OnChoreUnhidden?.Invoke(choreId);
	}
}