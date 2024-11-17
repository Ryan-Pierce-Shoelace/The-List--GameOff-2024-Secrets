namespace Interaction.InteractionCore
{
    public interface IInteractable
    {
        bool CanInteract();
        void Interact();
        InteractObjectSO GetInteractableObject();
        void TriggerFailedInteractionThought();
        void ToggleHighlight(bool toggle);
    }
}
