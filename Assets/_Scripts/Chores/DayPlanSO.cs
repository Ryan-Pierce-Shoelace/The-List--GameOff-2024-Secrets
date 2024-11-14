using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Horror.Chores
{
    [CreateAssetMenu(fileName = "New Day Plan", menuName = "Chores/Day Plan")]
    public class DayPlan : ScriptableObject
    {
        [FormerlySerializedAs("dayName")] public string DayName;
        [FormerlySerializedAs("chores")] public List<ChoreDataSO> Chores;
    }
}
