using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RyanPierce.Core
{
    [CreateAssetMenu(fileName = "New Bool SO", menuName = "SOVariable/Bool")]
    public class BoolVariable : ScriptableObject
    {
        public bool Value;
    }
}
