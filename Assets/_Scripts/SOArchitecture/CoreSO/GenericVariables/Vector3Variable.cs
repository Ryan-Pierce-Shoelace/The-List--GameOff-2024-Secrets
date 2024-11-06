using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RyanPierce.Core
{
    [CreateAssetMenu(fileName = "New Vector3 SO", menuName = "SOVariable/Vector3")]
    public class Vector3Variable : ScriptableObject
    {
        public Vector3 Value;
    }
}