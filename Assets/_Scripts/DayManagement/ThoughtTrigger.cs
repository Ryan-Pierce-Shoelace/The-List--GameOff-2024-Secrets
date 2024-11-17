using System;
using Horror.RoomNavigation;
using UnityEngine;

namespace DayManagement
{
    public class ThoughtTrigger : MonoBehaviour
    {
        [SerializeField] private DynamicThoughtSO dynamicThoughts;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.attachedRigidbody.TryGetComponent(out PlayerNavigator navigator))
            {
                TriggerRandomThought();
            }
        }

        public void TriggerRandomThought()
        {
            dynamicThoughts.PlayThought();
        }
    }
}
