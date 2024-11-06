using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RyanPierce.Core
{
    [CreateAssetMenu(fileName = "New String SO", menuName = "SOVariable/String")]
    public class StringVariable : ScriptableObject
    {
        public string Value;
    }
}