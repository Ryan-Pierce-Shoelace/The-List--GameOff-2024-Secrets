using System;
using System.Collections.Generic;
using Horror.Chores;
using UnityEngine;

namespace DayManagement
{
    public class DaySequenceManager : MonoBehaviour
    {
        [SerializeField] private DayPlan dayPlan;

    
        private ChoreManager choreManager;

        
        
    }

    [Serializable]
    public class HorrorTextEvent
    {
        public ChoreDataSO Chore;
        public float Freqency;
    }

}
