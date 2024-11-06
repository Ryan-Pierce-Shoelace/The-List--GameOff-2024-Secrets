using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RyanPierce.Core
{
    [CreateAssetMenu(fileName = "New Int SO", menuName = "SOVariable/Int")]
    public class IntVariable : ScriptableObject
    {
        public int Value;
    }
}