using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RyanPierce.Core
{
    [CreateAssetMenu(fileName = "New Float SO", menuName = "SOVariable/Float")]
    public class FloatVariable : ScriptableObject
    {
        public float Value;
    }
}