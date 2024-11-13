using UnityEngine;

namespace Interaction
{
    public class PickupInteractable : CleanInteractable
    {
        [SerializeField] private bool removeOnPickup;
        public override void Interact()
        {
            interactionManager.CollectObject(interactObjectSO);
            if(removeOnPickup)
                base.Interact();
        }
    }
}