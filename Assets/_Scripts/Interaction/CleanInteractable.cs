using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Horror.InteractionSystem
{
    public class CleanInteractable : BaseObject_Interactable
    {
        public override void Interact()
        {
            base.Interact();
            gameObject.SetActive(false);
        }
    }
}