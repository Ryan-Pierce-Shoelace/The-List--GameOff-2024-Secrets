using Horror.Chores;
using Interaction.InteractionCore;

namespace Interaction
{
    public class CleanInteractable : BaseObjectInteractable
    {

        public override void Interact()
        {
            base.Interact();
            gameObject.SetActive(false);
        }
    }
}