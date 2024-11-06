using UnityEngine;
namespace RyanPierce.Events
{
    [CreateAssetMenu (fileName = "New Void Event", menuName = "VLGameEvents/Void Event")]
    public class VoidEvent : BaseGameEvent<Void>
    {
        public void Raise() => Raise(new Void());
    }
}