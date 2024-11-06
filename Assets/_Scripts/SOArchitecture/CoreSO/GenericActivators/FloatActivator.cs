using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RyanPierce.Events;
namespace RyanPierce.Core
{
    [CreateAssetMenu(fileName = "New Float SO", menuName = "SOVariable/Float")]
    public class FloatActivator : ScriptableObject
    {
        private float value;

        [SerializeField] private FloatEvent activateEvent;

        public float Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
            }
        }
    }
}

