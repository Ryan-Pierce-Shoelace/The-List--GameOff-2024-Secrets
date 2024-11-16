using Interaction.InteractionCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interaction
{
    public class ToggleInteractable : BaseObjectInteractable
    {
        [SerializeField] private GameObject target;
        [SerializeField] private bool cycle;
        public override void Interact()
        {
            base.Interact();
            target.SetActive(!target.activeInHierarchy);

            if(cycle == false)
            {
                this.enabled = false;
            }
        }
    }
}