using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RyanPierce.Core
{
    [CreateAssetMenu(fileName = "New Vector2Int SO", menuName = "SOVariable/Vector2Int")]
    public class Vector2IntVariable : ScriptableObject
    {
        public Vector2Int Value;
    }
}