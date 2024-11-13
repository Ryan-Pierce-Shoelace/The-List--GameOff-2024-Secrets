using RyanPierce.Events;
using UnityEngine;

namespace Horror.InteractionSystem
{
    public abstract class BaseObject_Interactable : MonoBehaviour, IInteractable
    {
        [SerializeField] protected InteractObjectSO interactObjectSO;
        [SerializeField] protected GameObject highlightObject;

        [SerializeField] protected VoidEvent interactionEvent;

        [SerializeField] protected InteractionManager interactionManager;
        [SerializeField] protected InteractObjectSO[] inventoryRequirements;

        protected virtual void Start()
        {
            ToggleHighlight(false);
        }
        public virtual bool CanInteract()
        {
            if(interactionManager == null)
            {
                Debug.LogError(transform.name + " doesnt have an interaction manager assigned");
                return false;
            }
            if(interactObjectSO == null)
            {
                Debug.LogError(transform.name + " Has a null Interaction Object");
                return false;
            }

            for(int i = 0; i < inventoryRequirements.Length; i++)
            {
                if (!interactionManager.HasObject(inventoryRequirements[i]))
                {
                    return false; // did not have item
                }
            }

            return true;
        }
        public virtual void Interact()
        {
            interactionEvent?.Raise();
        }
        public void ToggleHighlight(bool toggleOn) => highlightObject.SetActive(toggleOn);
        public InteractObjectSO GetInteractableObject() => interactObjectSO;

        public virtual string GetFailedInteractionThought()
        {
            for (int i = 0; i < inventoryRequirements.Length; i++)
            {
                if (!interactionManager.HasObject(inventoryRequirements[i]))
                {
                    return $"I dont have the {inventoryRequirements[i].name}"; // did not have item
                }
            }
            return "Im a failure";
        }
    }
}
