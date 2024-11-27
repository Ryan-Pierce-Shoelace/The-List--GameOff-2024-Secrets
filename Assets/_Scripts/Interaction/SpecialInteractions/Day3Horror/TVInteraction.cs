using Horror.Chores;
using System.Collections;
using System.Collections.Generic;
using UI.Thoughts;
using UnityEngine;
namespace Interaction.InteractionCore
{

    public class TVInteraction : MonoBehaviour, IInteractable
    {
        [Header("Fail Interact State")]
        [SerializeField] private ChoreDataSO requiredChore;
        [SerializeField] private DynamicThoughtSO failThought;


        private bool isWatchingTV;

        public bool CanInteract()
        {
            throw new System.NotImplementedException();
        }

        public InteractObjectSO GetInteractableObject()
        {
            throw new System.NotImplementedException();
        }

        public void Interact()
        {
            throw new System.NotImplementedException();
        }

        public bool IsActive()
        {
            throw new System.NotImplementedException();
        }

        public void ToggleHighlight(bool toggle)
        {
            throw new System.NotImplementedException();
        }

        public void TriggerFailedInteractionThought()
        {
            throw new System.NotImplementedException();
        }

        public void TryTriggerSuccessInteractionThought()
        {
            throw new System.NotImplementedException();
        }
    }

}