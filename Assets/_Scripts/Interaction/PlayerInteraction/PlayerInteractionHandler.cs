using Horror.InputSystem;
using Interaction.InteractionCore;
using System.Threading.Tasks;
using UnityEngine;
using Utilities;

namespace Interaction.PlayerInteraction
{
    public class PlayerInteractionHandler : MonoBehaviour
    {
        [SerializeField] private GameObject interactButtonPrompt;

        [SerializeField] private InputReader input;
        [SerializeField] private LayerMask interactableLayer;
        [SerializeField] private float searchRadius;
        private IInteractable currentInteractable;

        [SerializeField] private Animator animator;
        private TaskCompletionSource<bool> animCompleteSource;

        private void Start()
        {
            input.InteractEvent += TryInteracting;
        }

        private void TryInteracting()
        {
            if (currentInteractable == null)
            {
                return;
            }
            
            if(currentInteractable.CanInteract())
            {
                InteractTarget(currentInteractable);
            }
            else
            {
                StaticEvents.TriggerThought(currentInteractable.GetFailedInteractionThought());
            }
        }

        private async void InteractTarget(IInteractable target)
        {
            await RunInteractAnimation(target.GetInteractableObject().AnimationName, target.GetInteractableObject().InteractionTime);
            target.Interact();
        }

        private async Task RunInteractAnimation(string animationName, float duration)
        {
            animator.Play(animationName);
            
            // Calculate playback speed to fit the desired duration
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            float originalDuration = stateInfo.length;
            animator.speed = duration > 0 ? originalDuration / duration : 1f;

            
            // Initialize the TaskCompletionSource
            animCompleteSource = new TaskCompletionSource<bool>();

            // Wait for the event to complete the Task
            await animCompleteSource.Task;

            // Reset the animator speed after the animation is finished
            animator.speed = 1.0f;
        }

        public void InteractAnimationComplete()
        {
            animCompleteSource?.TrySetResult(true);
        }

        private void LateUpdate()
        {
            FindClosestInteractable();
        }

        private void FindClosestInteractable()
        {
            Collider2D col = Physics2D.OverlapCircle(transform.position, searchRadius, interactableLayer);

            if (!col)
            {
                ClearInteractable();
                return;
            }
                

            if(col.TryGetComponent(out IInteractable interactable))
            {
                SetInteractable(interactable);
            }
            else
            {
                ClearInteractable();
            }
        }

        private void SetInteractable(IInteractable newInteractable)
        {
            if (currentInteractable != null && currentInteractable != newInteractable)
                currentInteractable.ToggleHighlight(false);

            currentInteractable = newInteractable;
            currentInteractable.ToggleHighlight(true);
            StaticEvents.DisplayInteractable(currentInteractable.GetInteractableObject().name);
            interactButtonPrompt.SetActive(true);
        }

        private void ClearInteractable()
        {
            currentInteractable?.ToggleHighlight(false);
            currentInteractable = null;
            StaticEvents.DisplayInteractable("");
            interactButtonPrompt.SetActive(false);
        }
    }
}
