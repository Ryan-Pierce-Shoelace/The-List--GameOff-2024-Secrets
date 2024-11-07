using System;

namespace Text_Display
{
	public static class ThoughtEvent
	{
		public static event Action<string> OnThoughtTriggered;



		public static void TriggerThought(string text) => OnThoughtTriggered?.Invoke(text);
	}
}

