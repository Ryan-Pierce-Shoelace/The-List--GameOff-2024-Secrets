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
                currentInteractable.TryTriggerSuccessInteractionThought();
            }
            else
            {
                currentInteractable.TriggerFailedInteractionThought();
            }
        }

        private async void InteractTarget(IInteractable target)
        {
            await RunInteractAnimation("Interact"); //In the future we can add more animations but for now. Just hard pass Interact
            target.Interact();
        }

        private async Task RunInteractAnimation(string animationName)
        {
            animator.Play(animationName);        
            // Initialize the TaskCompletionSource
            animCompleteSource = new TaskCompletionSource<bool>();
            // Wait for the event to complete the Task
            await animCompleteSource.Task;
        }

        public void AnimationComplete()
        {
            animCompleteSource?.TrySetResult(true);
        }

        private void LateUpdate()
        {
            if (input.IsDisabled())
            {
                ClearInteractable();
                return;
            }
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
                if(!interactable.IsActive())
                {
                    ClearInteractable();
                    return;
                }

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
            {
                currentInteractable.ToggleHighlight(false);
            }
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
