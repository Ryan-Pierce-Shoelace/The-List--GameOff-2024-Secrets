using Interaction.InteractionCore;
using RyanPierce.Events;
using UnityEngine;
using UnityEngine.Serialization;

namespace Interaction
{
    public class GUIInteractable : BaseObjectInteractable
    {
        [FormerlySerializedAs("OpenGUIEvent")] [SerializeField] private VoidEvent openGUIEvent;
        public override void Interact()
        {
            base.Interact();
            openGUIEvent?.Raise();
        }
    }
}