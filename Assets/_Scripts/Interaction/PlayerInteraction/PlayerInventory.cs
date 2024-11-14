using System.Collections.Generic;
using Interaction.InteractionCore;
using UnityEngine;

namespace Interaction.PlayerInteraction
{
    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField] private InteractionManager interactionManager;

        [SerializeField] private List<InteractObjectSO> dayOneObjects;
        private void Start()
        {
            interactionManager.InitDailyInventory(dayOneObjects);
        }
    }
}
