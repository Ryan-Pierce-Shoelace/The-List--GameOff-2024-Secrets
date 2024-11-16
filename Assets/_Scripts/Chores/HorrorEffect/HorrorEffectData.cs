using UnityEngine;

namespace Horror.Chores.HorrorEffect
{
    [System.Serializable]
    public class HorrorEffectData
    {
        public string OverrideText;
        public float Duration;
        public Color TextColor = Color.red;
        public float ShakeAmount = 5f;
        public int ShakeVibrato = 10;
    }
}
