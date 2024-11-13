using System.Collections.Generic;
using UnityEngine;

namespace Interaction.InteractionCore
{
    [CreateAssetMenu(fileName = "Interaction Manager", menuName = "HorrorGame/Interaction Manager")]
    public class InteractionManager : ScriptableObject
    {
        private Dictionary<InteractObjectSO, bool> inventory;
        public void Clear() => inventory.Clear();

        public void InitDailyInventory(List<InteractObjectSO> dailyObjects)
        {
            inventory = new Dictionary<InteractObjectSO, bool>();

            foreach (InteractObjectSO obj in dailyObjects)
            {
                inventory.Add(obj, false);
            }
        }

        public bool HasObject(InteractObjectSO obj)
        {
            return inventory.ContainsKey(obj) && inventory[obj];
        }

        public void CollectObject(InteractObjectSO obj)
        {
            if(!inventory.ContainsKey(obj)) return;
            inventory[obj] = true;
        }
        public void RemoveObject(InteractObjectSO obj)
        {
            if (!inventory.ContainsKey(obj)) return;
            inventory[obj] = false;
        }

    }
}
