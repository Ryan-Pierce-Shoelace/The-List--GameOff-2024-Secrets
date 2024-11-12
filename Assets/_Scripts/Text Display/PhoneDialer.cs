using System;
using Shoelace.Audio;
using Shoelace.Audio.XuulSound;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Text_Display
{
	public class PhoneDialer : MonoBehaviour
	{
		[SerializeField] private SoundConfig buttonPressSound;
		[SerializeField] private SoundConfig dialToneSound;
		
		
		[SerializeField] private TMP_Text displayText;
		[SerializeField] private Button dialButton;
		[SerializeField] private Button backButton;
		[SerializeField] private Button clearButton;

		[SerializeField] private PhoneButton[] phoneButtons;

		private string currentNumber = "";
		private const int MAX_DIGITS = 10;

		private ISoundPlayer dialTonePlayer;
		
		private void Start()
		{
			InitializeButtons();
			UpdateDisplay();
			dialTonePlayer = AudioManager.Instance.CreateSound(dialToneSound);
		}

		private void OnEnable()
		{
			dialTonePlayer?.Play();
		}

		private void OnDisable()
		{
			ClearNumber();
			dialTonePlayer?.Stop();
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
			currentNumber += number.ToString();
			UpdateDisplay();
			
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

			StaticEvents.DialPhone(currentNumber);

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


		

		private string FormatPhoneNumber(string number)
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