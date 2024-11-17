using Interaction.InteractionCore;
using Shoelace.Audio.XuulSound;
using UI.Thoughts;
using UnityEngine;

namespace Interaction
{
    public class VoidInteractable : BaseObjectInteractable
    {
        [SerializeField] private DynamicThoughtSO triggerThought;
        [SerializeField] private SoundConfig triggerSFX;

        public override bool CanInteract()
        {
            return true;
        }

        public override void Interact()
        {
            if(triggerThought != null)
            {
                triggerThought.PlayThought();
            }
            if(triggerSFX != null)
            {
                AudioManager.Instance.PlayOneShot(triggerSFX);
            }
            
            base.Interact();
        }
    }
}
