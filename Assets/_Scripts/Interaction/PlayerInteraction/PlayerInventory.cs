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
            PickupInteractable[] pickups = FindObjectsOfType<PickupInteractable>();
            List<InteractObjectSO> scenePickups = new List<InteractObjectSO>();

            for (int i = 0; i < pickups.Length; i++)
            {
                if (scenePickups.Contains(pickups[i].GetInteractableObject()))
                    continue;

                scenePickups.Add(pickups[i].GetInteractableObject());
            }

            interactionManager.InitDailyInventory(scenePickups);
        }
    }
}
