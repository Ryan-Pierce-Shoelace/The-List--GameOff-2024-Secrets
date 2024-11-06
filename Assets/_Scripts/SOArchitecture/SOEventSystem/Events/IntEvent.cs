using UnityEngine;
namespace RyanPierce.Events
{
    [CreateAssetMenu (fileName = "New Int Event", menuName = "VLGameEvents/Int Event")]
    public class IntEvent : BaseGameEvent<int>
    {
        public void Raise() => Raise(new int());
    }
}