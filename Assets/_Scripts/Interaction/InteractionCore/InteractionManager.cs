using System.Collections.Generic;
using UnityEngine;

namespace Horror.InteractionSystem
{
    [CreateAssetMenu(fileName = "Interaction Manager", menuName = "HorrorGame/Interaction Manager")]
    public class InteractionManager : ScriptableObject
    {
        private Dictionary<InteractObjectSO, bool> Inventory;
        public void Clear() => Inventory.Clear();

        public void InitDailyInventory(List<InteractObjectSO> dailyObjects)
        {
            Inventory = new Dictionary<InteractObjectSO, bool>();

            foreach (InteractObjectSO obj in dailyObjects)
            {
                Inventory.Add(obj, false);
            }
        }

        public bool HasObject(InteractObjectSO obj)
        {
            if(!Inventory.ContainsKey(obj))
                return false;

            return Inventory[obj];
        }

        public void CollectObject(InteractObjectSO obj)
        {
            if(!Inventory.ContainsKey(obj)) return;
            Inventory[obj] = true;
        }
        public void RemoveObject(InteractObjectSO obj)
        {
            if (!Inventory.ContainsKey(obj)) return;
            Inventory[obj] = false;
        }

    }
}
