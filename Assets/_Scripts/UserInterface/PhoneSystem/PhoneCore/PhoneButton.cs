using UnityEngine;
using UnityEngine.UI;

namespace UserInterface
{
	public class PhoneButton : MonoBehaviour
	{
		[SerializeField] private int value;
		[SerializeField] private Button myButton;

		public int Value => value;
		public Button MyButton => myButton;

		private void OnValidate()
		{
			if (myButton == null)
			{
				myButton = GetComponent<Button>();
			}
		}
	}
}
