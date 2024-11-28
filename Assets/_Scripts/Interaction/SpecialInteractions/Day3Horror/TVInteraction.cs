using DG.Tweening;
using Horror;
using Horror.Chores;
using Horror.InputSystem;
using RyanPierce.Events;
using Shoelace.Audio.XuulSound;
using System.Threading.Tasks;
using UI.Thoughts;
using UnityEngine;
using UnityEngine.UI;

namespace Interaction.InteractionCore
{

    public class TVInteraction : MonoBehaviour, IInteractable
    {
        [Header("Object Data")]
        [SerializeField] private InteractObjectSO couchObject;
        [SerializeField] private GameObject highlight;
        [SerializeField] private Transform playerSitTransform;
        [SerializeField] private ToggleInteractable tvInteractable;
        [Header("Interact State")]
        [SerializeField] private ChoreProgressor watchTV;
        [SerializeField] private ChoreProgressor drinkToForget;
        [SerializeField] private DynamicThoughtSO failThought;
        [Header("Player Ref")]
        [SerializeField] private Transform playerTransform;
        [SerializeField] private InputReader inputReader;
        [SerializeField] private BoolEvent playerMovementToggle;
        [SerializeField] private SpriteRenderer playerSprite;

        [Header("Horror Sequence")]
        [SerializeField] private RawImage drunkOverlay;
        [SerializeField] private SoundConfig drinkSFX;
        [SerializeField] private Animator tvAnimation;
        [SerializeField] private SoundConfig crackSFX;
        [SerializeField] private ChoreRevealer revealChores;

        [SerializeField] private GameObject[] revealObjects, hideObjects;

        private bool isWatchingTV;
        private TaskCompletionSource<bool> tvAnimComplete;

        private void Start()
        {
            ToggleHighlight(false);
            isWatchingTV = false;

            foreach (var item in revealObjects)
            {
                item.SetActive(false);
            }
            foreach (var item in hideObjects)
            {
                item.SetActive(true);
            }
        }

        public void Interact()
        {
            if(isWatchingTV)
            {
                drinkToForget.ProgressChore();
                AudioManager.Instance.PlayOneShot(drinkSFX);
                drunkOverlay.color = new Color(1f, 1f, 1f, drinkToForget.GetChoreProgress());

                if (drinkToForget.GetChoreState() == ChoreState.Completed)
                {
                    inputReader.DisableAllInput();
                    isWatchingTV = false;
                    RunTVHorrorSequence();
                }
            }
            else
            {
                tvInteractable.ForceToggle(true);
                watchTV.ProgressChore();
                drunkOverlay.gameObject.SetActive(true);
                drunkOverlay.color = new Color(1f, 1f, 1f, 0f);

                isWatchingTV = true;

                playerMovementToggle?.Raise(false);
                playerTransform.position = playerSitTransform.position;
                playerSprite.color = Color.black;
            }
        }

        public void AnimationComplete()
        {
            tvAnimComplete?.TrySetResult(true);
        }

        private async void RunTVHorrorSequence()
        {


            tvAnimation.gameObject.SetActive(true);
            tvAnimComplete = new TaskCompletionSource<bool>();
            await tvAnimComplete.Task;

            AudioManager.Instance.PlayOneShot(crackSFX);


            await FadeTransition.Instance.ToggleFadeTransition(true, .2f);
            ChoreEvents.ClearList();
            foreach (var item in revealObjects)
            {
                item.SetActive(true);
            }
            foreach (var item in hideObjects)
            {
                item.SetActive(false);
            }
            await FadeTransition.Instance.ToggleFadeTransition(false, .2f);
            drunkOverlay.DOFade(0f, 1f);
            await Task.Delay(1000);

            revealChores?.TryRevealNewChores();
            inputReader.EnableGameplayInput();
            playerMovementToggle?.Raise(true);
            playerSprite.color = Color.white;
            enabled = false;
        }

        private void OnApplicationQuit()
        {
            AnimationComplete();
        }


        public bool CanInteract()
        {


            if (isWatchingTV)
                return true;

            if (couchObject == null)
            {
                Debug.LogError(transform.name + " Has a null Interaction Object");
                return false;
            }

            if (watchTV == null) return false;

            bool isChoreStateValid = watchTV.GetChoreState() == ChoreState.Available || watchTV.GetChoreState() == ChoreState.Completed;


            if (isChoreStateValid)
            {
                return true;
            }

            return false;
        }

        public InteractObjectSO GetInteractableObject()
        {
            return couchObject;
        }

       
        public bool IsActive()
        {
            return this.enabled;
        }

        public void ToggleHighlight(bool toggle)
        {
            highlight.SetActive(toggle);
        }

        public void TriggerFailedInteractionThought()
        {
            failThought?.PlayThought();
        }

        public void TryTriggerSuccessInteractionThought()
        {
        }
    }

}