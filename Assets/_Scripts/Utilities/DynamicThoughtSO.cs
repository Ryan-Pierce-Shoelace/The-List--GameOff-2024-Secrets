using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace UI.Thoughts
{
	[CreateAssetMenu(fileName = "New Dynamic Thought", menuName = "HorrorGame/DynamicThought")]
	public class DynamicThoughtSO : ScriptableObject
	{
		[SerializeField] private List<string> possibleThoughts = new List<string>();
		private ArrayShuffler<string> thoughtShuffler;
		
		private void OnEnable()
		{
			thoughtShuffler = new ArrayShuffler<string>(possibleThoughts);
		}
		
		private string GetRandomThought()
		{
			return possibleThoughts.Count switch
			{
				0 => "",
				1 => possibleThoughts[0],
				_ => thoughtShuffler.GetNext()
			};
		}
		
		public void PlayThought()
		{
			if (possibleThoughts.Count == 0) return;

			string thought = GetRandomThought();
			StaticEvents.TriggerThought(thought);
		}
	}
}