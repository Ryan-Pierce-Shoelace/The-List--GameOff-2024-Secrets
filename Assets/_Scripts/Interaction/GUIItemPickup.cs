using Horror.Chores;
using Interaction.InteractionCore;
using Shoelace.Audio.XuulSound;
using UI.Thoughts;
using UnityEngine;
using UnityEngine.UI;

namespace Interaction.InteractionCore
{
    public class GUIItemPickup : MonoBehaviour
    {
        
        [SerializeField] private Button clickableButton;
        [SerializeField] private float alphaHitThreshold = .2f;

        [SerializeField] private InteractionManager interactionManager;

        [SerializeField] private InteractObjectSO pickUpObject;
        [SerializeField] private SoundConfig pickupSFX;

        [SerializeField] private ChoreProgressor choreProgressor;
        [SerializeField] private DynamicThoughtSO choreFailThought;

        public InteractObjectSO GetItemPickup() => pickUpObject;

        private void Start()
        {
            clickableButton.image.alphaHitTestMinimumThreshold = alphaHitThreshold;
            clickableButton.onClick.AddListener(() => GrabFridgeItem());
        }

        private void GrabFridgeItem()
        {
            if(choreProgressor != null)
            {
                bool isChoreStateValid = choreProgressor.GetChoreState() == ChoreState.Available || choreProgressor.GetChoreState() == ChoreState.Completed;
                if (choreProgressor != null && isChoreStateValid)
                {
                    choreProgressor?.ProgressChore();
                }
                else
                {
                    choreFailThought?.PlayThought();
                    return;
                }
            }


            if (pickupSFX != null)
            {
                AudioManager.Instance.PlayOneShot(pickupSFX);
            }
            if(pickUpObject != null)
            {
                interactionManager.CollectObject(pickUpObject);

            }
            clickableButton.interactable = false;
            clickableButton.image.raycastTarget = false;

            gameObject.SetActive(false);
            
        }


    }
}
