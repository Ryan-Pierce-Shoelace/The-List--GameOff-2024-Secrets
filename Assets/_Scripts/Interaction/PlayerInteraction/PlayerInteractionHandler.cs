using Interaction.InteractionCore;
using Project.Input.InputSystem;
using UnityEngine;

namespace Interaction.PlayerInteraction
{
    public class PlayerInteractionHandler : MonoBehaviour
    {
        [SerializeField] private GameObject interactButtonPrompt;

        [SerializeField] private InputReader input;
        [SerializeField] private LayerMask interactableLayer;
        [SerializeField] private float searchRadius;
        private IInteractable currentInteractable;
        private void Start()
        {
            input.InteractEvent += TryInteracting;
        }

        private void TryInteracting()
        {
            if(currentInteractable.CanInteract())
            {
                currentInteractable.Interact();
            }
            else
            {
                StaticEvents.TriggerThought(currentInteractable.GetFailedInteractionThought());
            }
        }

        private void LateUpdate()
        {
            FindClosestInteractable();
        }

        private void FindClosestInteractable()
        {
            Collider2D col = Physics2D.OverlapCircle(transform.position, searchRadius, interactableLayer);

            if (col == null)
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
            if(currentInteractable != null)
                currentInteractable.ToggleHighlight(false);
            currentInteractable = null;
            StaticEvents.DisplayInteractable("");
            interactButtonPrompt.SetActive(false);
        }
    }
}
