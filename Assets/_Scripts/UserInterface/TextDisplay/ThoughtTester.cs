using UnityEngine;
using Utilities;

namespace Horror.UserInterface.TextDisplay
{
	public class ThoughtTester : MonoBehaviour
	{
		[Header("Test Settings")]
		[SerializeField] [TextArea(2, 5)] private string thoughtToTest = "type the thought to test here";

		[SerializeField] private KeyCode triggerKey = KeyCode.E;


		private void Update()
		{
			if (Input.GetKeyDown(triggerKey))
			{
				if (!string.IsNullOrEmpty(thoughtToTest))
				{
					StaticEvents.TriggerThought(thoughtToTest);
				}
			}
		}
	}
}