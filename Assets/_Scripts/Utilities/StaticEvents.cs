using System;

namespace Utilities
{
	public static class StaticEvents
	{
		public static event Action<string> OnThoughtTriggered;
		public static event Action<string> OnNumberDialed;
		public static event Action<string> OnInteractableHovered;

		public static void TriggerThought(string text) => OnThoughtTriggered?.Invoke(text);
		public static void DisplayInteractable(string text) => OnInteractableHovered?.Invoke(text);
		public static void DialPhone(string text) => OnNumberDialed?.Invoke(text);
	}
}