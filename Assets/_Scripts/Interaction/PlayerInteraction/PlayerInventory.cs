using System.Collections.Generic;
using Interaction.InteractionCore;
using UnityEngine;

namespace Interaction.PlayerInteraction
{
    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField] private InteractionManager interactionManager;

        private void Start()
        {
            PickupInteractable[] groundPickups = FindObjectsOfType<PickupInteractable>();
            GUIItemPickup[] uiPickups = FindObjectsOfType<GUIItemPickup>();
            List<InteractObjectSO> scenePickups = new List<InteractObjectSO>();

            for (int i = 0; i < groundPickups.Length; i++)
            {
                if (scenePickups.Contains(groundPickups[i].GetInteractableObject()))
                    continue;

                scenePickups.Add(groundPickups[i].GetInteractableObject());
            }
            for (int i = 0; i < uiPickups.Length; i++)
            {
                if (scenePickups.Contains(uiPickups[i].GetInteractableObject()))
                    continue;

                scenePickups.Add(uiPickups[i].GetInteractableObject());
            }
            interactionManager.InitDailyInventory(scenePickups);
        }
    }
}
