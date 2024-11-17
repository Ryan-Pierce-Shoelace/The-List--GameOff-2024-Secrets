using Interaction.InteractionCore;
using UnityEngine;

namespace Interaction
{
    public class ChangeInteractable : BaseObjectInteractable
    {
        [SerializeField] private SpriteRenderer targetRenderer;
        [SerializeField] private Sprite newSprite;
        private Sprite oldSprite;
        [SerializeField] private bool repeatCycle;
        private bool isChanged;
        protected override void Start()
        {
            base.Start();
            oldSprite = targetRenderer.sprite;
            isChanged = false;
        }


        public override void Interact()
        {
            base.Interact();

            if(!isChanged)
            {
                targetRenderer.sprite = newSprite;

                if(!repeatCycle)
                {
                    ToggleHighlight(false);
                    this.enabled = false;
                }
                    

            }
            else if(repeatCycle)
            {
                targetRenderer.sprite = oldSprite;
            }
        }
    }
}
