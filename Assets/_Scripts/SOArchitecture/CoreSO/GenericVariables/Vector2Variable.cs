using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RyanPierce.Core
{
    [CreateAssetMenu(fileName = "New Vector2 SO", menuName = "SOVariable/Vector2")]
    public class Vector2Variable : ScriptableObject
    {
        public Vector2 Value;
    }
}