using UnityEngine;
namespace RyanPierce.Events
{
    [CreateAssetMenu(fileName = "New Bool Event", menuName = "VLGameEvents/Bool Event")]
    public class BoolEvent : BaseGameEvent<bool>
    {
        public void Raise() => Raise(new bool());
    }
}