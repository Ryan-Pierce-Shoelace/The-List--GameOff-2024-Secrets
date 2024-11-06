using Shoelace.Events;
using UnityEngine;
namespace Shoelace.Audio
{
    [CreateAssetMenu(fileName = "New FSound Event", menuName = "GameEvents/FSound Event")]
    public class FSoundEvent : BaseGameEvent<FSoundData>
    {
        public void Raise() => Raise(new FSoundData());
    }
}
