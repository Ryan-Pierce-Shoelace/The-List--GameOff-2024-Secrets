using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Horror.InteractionSystem
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
