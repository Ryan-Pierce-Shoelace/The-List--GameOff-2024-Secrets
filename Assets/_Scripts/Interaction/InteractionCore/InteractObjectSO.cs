using UnityEngine;
namespace Horror.InteractionSystem
{
    [CreateAssetMenu(fileName = "New Interactable Object", menuName = "HorrorGame/Interactable SO")]
    public class InteractObjectSO : ScriptableObject
    {
        public string animationString;
        public float interactionTime;
    }
}