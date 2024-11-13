using System.Collections.Generic;
using UnityEngine;
namespace Horror.InteractionSystem
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
