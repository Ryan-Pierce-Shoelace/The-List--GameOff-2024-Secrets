using RyanPierce.Events;
using UnityEngine;
namespace Horror.InteractionSystem
{
    public class GUIInteractable : BaseObject_Interactable
    {
        [SerializeField] private VoidEvent OpenGUIEvent;
        public override void Interact()
        {
            base.Interact();
            OpenGUIEvent?.Raise();
        }
    }
}