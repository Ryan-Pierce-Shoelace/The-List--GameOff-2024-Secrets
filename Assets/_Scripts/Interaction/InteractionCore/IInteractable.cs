namespace Interaction.InteractionCore
{
    public interface IInteractable
    {
        bool CanInteract();
        void Interact();
        InteractObjectSO GetInteractableObject();
        string GetFailedInteractionThought();
        void ToggleHighlight(bool toggle);
    }
}
