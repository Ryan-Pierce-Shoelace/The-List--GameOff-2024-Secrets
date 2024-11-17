using System.Linq;
using Horror.Chores;
using RyanPierce.Events;
using UnityEngine;

namespace Interaction.InteractionCore
{
    public abstract class BaseObjectInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] protected InteractObjectSO interactObjectSO;
        [SerializeField] protected GameObject highlightObject;

        [SerializeField] protected VoidEvent interactionEvent;

        [SerializeField] protected InteractionManager interactionManager;
        [SerializeField] protected InteractObjectSO[] inventoryRequirements;

        private ChoreProgressor choreProgressor;

        protected virtual void Start()
        {
            ToggleHighlight(false);
            choreProgressor = GetComponent<ChoreProgressor>();
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

            bool isChoreStateValid = choreProgressor.GetChoreState() == ChoreState.Available || choreProgressor.GetChoreState() == ChoreState.Completed;

            if (choreProgressor != null && isChoreStateValid)
            {
                return inventoryRequirements.All(t => interactionManager.HasObject(t));
            }

            return false;
        }
        public virtual void Interact()
        {
            interactionEvent?.Raise();
            choreProgressor?.ProgressChore();
        }
        public void ToggleHighlight(bool toggleOn) => highlightObject.SetActive(toggleOn);
        public InteractObjectSO GetInteractableObject() => interactObjectSO;

        public virtual string GetFailedInteractionThought()
        {
            foreach (InteractObjectSO t in inventoryRequirements)
            {
                if (!interactionManager.HasObject(t))
                {
                    return $"I dont have the {t.name}"; // did not have item
                }
            }

            return "Im a failure";
        }
    }
}
