using Horror.Chores;
using Interaction.InteractionCore;

namespace Interaction
{
    public class CleanInteractable : BaseObjectInteractable
    {
        private ChoreProgressor choreProgressor;

        protected override void Start()
        {
            base.Start();
            choreProgressor = GetComponent<ChoreProgressor>();
        }

        public override void Interact()
        {
            base.Interact();
            choreProgressor?.ProgressChore();
            gameObject.SetActive(false);
        }
    }
}