using UnityEngine;
using UnityEngine.Serialization;

namespace Interaction.InteractionCore
{
    [CreateAssetMenu(fileName = "New Interactable Object", menuName = "HorrorGame/Interactable SO")]
    public class InteractObjectSO : ScriptableObject
    {
        [FormerlySerializedAs("animationString")] public string AnimationName = "Interact";
    }
}