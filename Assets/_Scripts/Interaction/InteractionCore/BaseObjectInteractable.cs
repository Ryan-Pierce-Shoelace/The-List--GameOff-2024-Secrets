using System.Linq;
using Horror.Chores;
using RyanPierce.Events;
using UI.Thoughts;
using UnityEngine;
using Utilities;

namespace Interaction.InteractionCore
{
	public abstract class BaseObjectInteractable : MonoBehaviour, IInteractable
	{
		[SerializeField] protected InteractObjectSO interactObjectSO;
		[SerializeField] protected GameObject highlightObject;

		[SerializeField] protected VoidEvent interactionEvent;

		[SerializeField] protected InteractionManager interactionManager;
		[SerializeField] protected InteractObjectSO[] inventoryRequirements;

		[SerializeField] protected DynamicThoughtSO failThought;

		private ChoreProgressor choreProgressor;

		protected virtual void Start()
		{
			ToggleHighlight(false);
			choreProgressor = GetComponent<ChoreProgressor>();
		}

		public virtual bool CanInteract()
		{
			if (interactionManager == null)
			{
				Debug.LogError(transform.name + " doesnt have an interaction manager assigned");
				return false;
			}

			if (interactObjectSO == null)
			{
				Debug.LogError(transform.name + " Has a null Interaction Object");
				return false;
			}

			if (choreProgressor == null) return true;
			
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

		public void ToggleHighlight(bool toggleOn)
		{
			if(highlightObject != null)
			{
                highlightObject.SetActive(toggleOn);
            }
		}
		public InteractObjectSO GetInteractableObject() => interactObjectSO;

		public virtual void TriggerFailedInteractionThought()
		{
			if (failThought != null)
			{
				failThought.PlayThought();
				return;
			}
			else
			{
				foreach (InteractObjectSO t in inventoryRequirements)
				{
					if (interactionManager.HasObject(t)) continue;

					StaticEvents.TriggerThought($"I dont have the {t.name}");
					return;
				}
			}


			StaticEvents.TriggerThought("Im a failure");
		}

        public virtual bool IsActive()
        {
			return enabled;
        }
    }
}