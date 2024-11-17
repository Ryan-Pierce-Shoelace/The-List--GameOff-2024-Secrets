using System.Collections;
using TMPro;
using UnityEngine;
using Utilities;

namespace Horror.UserInterface.TextDisplay
{
    public class TextDisplayManager : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TMP_Text interactableNameText;
        [SerializeField] private TextMeshProUGUI thoughtText;

        [Header("Thought Display Settings")]
        [SerializeField] private float thoughtDisplayTime = 3f;
        [SerializeField] private float thoughtFadeOutDuration = 1f;

        private Coroutine currentThoughtDisplayCoroutine;

        [Header("Thought Display Settings")]
        [SerializeField] private TMP_Text roomNameText;
        [SerializeField] private TMP_Text roomChoreCompletionText;
        [SerializeField] private float roomDisplayTime = 3f;
        [SerializeField] private float roomFadeOutDuration = 1f;

        private Coroutine roomDisplayCoroutine;

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
            if (roomNameText != null)
            {
                roomNameText.alpha = 0f;
            }
            if (roomChoreCompletionText != null)
            {
                roomChoreCompletionText.alpha = 0f;
            }
        }

        private void OnEnable()
        {
            StaticEvents.OnThoughtTriggered += DisplayThought;
            StaticEvents.OnInteractableHovered += DisplayInteractable;
            StaticEvents.OnRoomEntered += DisplayRoomData;
        }



        private void OnDisable()
        {
            StaticEvents.OnThoughtTriggered -= DisplayThought;
            StaticEvents.OnInteractableHovered -= DisplayInteractable;
            StaticEvents.OnRoomEntered -= DisplayRoomData;
        }

        private void DisplayInteractable(string interactableName)
        {
            if (interactableNameText == null)
                return;

            interactableNameText.alpha = string.IsNullOrEmpty(interactableName) ? 0f : 1f;
            interactableNameText.text = $"-{interactableName}-";
        }

        private void DisplayRoomData(string roomName, int completedChores, int totalChores)
        {
            if (roomNameText == null) return;
            if (roomChoreCompletionText == null) return;

            if (roomDisplayCoroutine != null)
            {
                StopCoroutine(roomDisplayCoroutine);
            }

            roomDisplayCoroutine = StartCoroutine(DisplayRoomDataCoroutine(roomName, completedChores, totalChores));
        }

        private IEnumerator DisplayRoomDataCoroutine(string roomName, int completedChores, int totalChores)
        {
            roomNameText.text = roomName;
            roomNameText.alpha = 1f;

            if (totalChores > 0)
            {
                roomChoreCompletionText.text = $"Chores Completed {completedChores}/{totalChores}";
                roomChoreCompletionText.alpha = 1f;
            }
            else
            {
                roomChoreCompletionText.text = "";
                roomChoreCompletionText.alpha = 0f;
            }

            yield return new WaitForSeconds(roomDisplayTime);


            float elapsedTime = 0f;
            while (elapsedTime < roomFadeOutDuration)
            {
                elapsedTime += Time.deltaTime;
                roomNameText.alpha = Mathf.Lerp(1f, 0f, elapsedTime / roomFadeOutDuration);
                if (totalChores > 0)
                {
                    roomChoreCompletionText.alpha = roomNameText.alpha;
                }

                yield return null;
            }


            roomNameText.alpha = 0f;
            roomChoreCompletionText.alpha = 0f;
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


            yield return new WaitForSeconds(thoughtDisplayTime);


            float elapsedTime = 0f;
            while (elapsedTime < thoughtFadeOutDuration)
            {
                elapsedTime += Time.deltaTime;
                thoughtText.alpha = Mathf.Lerp(1f, 0f, elapsedTime / thoughtFadeOutDuration);
                yield return null;
            }


            thoughtText.alpha = 0f;
        }
    }
}