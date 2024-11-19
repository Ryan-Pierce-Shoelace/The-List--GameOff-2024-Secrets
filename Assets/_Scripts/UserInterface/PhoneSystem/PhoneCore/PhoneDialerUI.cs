using Horror;
using Shoelace.Audio.XuulSound;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace UserInterface
{
	public class PhoneDialerUI : UIScreen
	{
		[Header("Sounds")]
		[SerializeField] private SoundConfig buttonPressSound;
		[SerializeField] private SoundConfig dialToneSound;

		[Header("UI elements")]
		[SerializeField] private TMP_Text displayText;

		[SerializeField] private Button dialButton;
		[SerializeField] private Button backButton;
		[SerializeField] private Button clearButton;

		[SerializeField] private PhoneButton[] phoneButtons;

		[SerializeField] private PhoneNumber[] callableNumbers;

		Dictionary<string, PhoneNumber> contacts;

		private string currentNumber = "";
		private const int MAX_DIGITS = 10;

		private ISoundPlayer dialTonePlayer;

		private void Start()
		{
			parentCanvas = GetComponentInParent<Canvas>();
			InitializeButtons();
			InitializeDayContacts();
			UpdateDisplay();
		}

        private void InitializeDayContacts()
        {
            contacts = new Dictionary<string, PhoneNumber>();

			for (int i = 0; i < callableNumbers.Length; i++)
			{
				contacts.Add(callableNumbers[i].phoneNumber, callableNumbers[i]);
			}
        }

        protected override void OnEnable()
		{
			base.OnEnable();

			dialTonePlayer ??= AudioManager.Instance.CreateSound(dialToneSound);

			dialTonePlayer?.Play();
		}

		protected override void OnDisable()
		{
			base.OnDisable();

			ClearNumber();
			dialTonePlayer?.Stop();
		}
		
		public void OpenPanel()
		{

			dialTonePlayer ??= AudioManager.Instance.CreateSound(dialToneSound);

			dialTonePlayer?.Play();
		}


		private void InitializeButtons()
		{
			foreach (PhoneButton button in phoneButtons)
			{
				int buttonValue = button.Value;
				button.MyButton.onClick.AddListener(() => OnNumberPressed(buttonValue));
			}

			if (dialButton != null)
			{
				dialButton.onClick.AddListener(DialNumber);
			}

			if (backButton != null)
			{
				backButton.onClick.AddListener(Backspace);
			}

			if (clearButton != null)
			{
				clearButton.onClick.AddListener(ClearNumber);
			}
		}


		private void OnNumberPressed(int number)
		{
			if (currentNumber.Length >= MAX_DIGITS) return;

			dialTonePlayer?.Stop();

			AudioManager.Instance.PlayOneShot(buttonPressSound);
			if(number >= 0)
			{
                currentNumber += number.ToString();
                UpdateDisplay();
            }
			
		}

		private void ClearNumber()
		{
			currentNumber = "";
			UpdateDisplay();
		}

		private void Backspace()
		{
			if (currentNumber.Length <= 0) return;
			currentNumber = currentNumber[..^1];
			UpdateDisplay();
		}

		private void DialNumber()
		{
			if (string.IsNullOrEmpty(currentNumber)) return;

			if(contacts.ContainsKey(currentNumber))
			{
				contacts[currentNumber].result.CallNumber();
			}

			ClearNumber();
		}

		private void UpdateDisplay()
		{
			if (displayText == null) return;

			if (string.IsNullOrEmpty(currentNumber))
			{
				displayText.text = "(   )    -    ";
				return;
			}

			displayText.text = FormatPhoneNumber(currentNumber);
		}


		private static string FormatPhoneNumber(string number)
		{
			return number.Length switch
			{
				0 => "(   )    -    ",
				1 or 2 or 3 => $"({number.PadRight(3)})    -    ",
				4 or 5 or 6 => $"({number[..3]}) {number[3..].PadRight(3)}-    ",
				7 or 8 or 9 or 10 => $"({number[..3]}) {number.Substring(3, 3)}-{number[6..].PadRight(4)}",
				_ => number
			};
		}
	}
}