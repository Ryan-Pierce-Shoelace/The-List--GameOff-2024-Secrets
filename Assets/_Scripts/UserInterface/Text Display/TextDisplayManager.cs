using System.Collections;
using TMPro;
using UnityEngine;
using Utilities;

namespace UserInterface.Text_Display
{
    public class TextDisplayManager : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TMP_Text interactableNameText;
        [SerializeField] private TextMeshProUGUI thoughtText;

        [Header("Thought Display Settings")]
        [SerializeField] private float displayTime = 3f;

        [SerializeField] private float fadeOutDuration = 1f;

        private Coroutine currentThoughtDisplayCoroutine;

        private void Start()
        {
            if (interactableNameText != null)
            {
                interactableNameText.alpha = 0f;
            }
            if (thoughtText != null)
            {
                thoughtText.alpha = 0f;
            }
        }

        private void OnEnable()
        {
            StaticEvents.OnThoughtTriggered += DisplayThought;
            StaticEvents.OnInteractableHovered += DisplayInteractable;
        }



        private void OnDisable()
        {
            StaticEvents.OnThoughtTriggered -= DisplayThought;
            StaticEvents.OnInteractableHovered -= DisplayInteractable;
        }

        private void DisplayInteractable(string interactableName)
        {
            if (interactableNameText == null)
                return;

            interactableNameText.alpha = string.IsNullOrEmpty(interactableName) ? 0f : 1f;
            interactableNameText.text = $"-{interactableName}-";
        }

        private void DisplayThought(string thought)
        {
            if (thoughtText == null) return;

            if (currentThoughtDisplayCoroutine != null)
            {
                StopCoroutine(currentThoughtDisplayCoroutine);
            }

            currentThoughtDisplayCoroutine = StartCoroutine(DisplayThoughtCoroutine(thought));
        }

        private IEnumerator DisplayThoughtCoroutine(string thought)
        {
            thoughtText.text = thought;
            thoughtText.alpha = 1f;


            yield return new WaitForSeconds(displayTime);


            float elapsedTime = 0f;
            while (elapsedTime < fadeOutDuration)
            {
                elapsedTime += Time.deltaTime;
                thoughtText.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeOutDuration);
                yield return null;
            }


            thoughtText.alpha = 0f;
        }
    }
}