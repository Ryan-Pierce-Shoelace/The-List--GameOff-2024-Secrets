using System;

public static class StaticEvents
{
	public static event Action<string> OnThoughtTriggered;
	public static event Action<string> OnNumberDialed; 



	public static void TriggerThought(string text) => OnThoughtTriggered?.Invoke(text);
		
	public static void DialPhone(string text) => OnNumberDialed?.Invoke(text);
}