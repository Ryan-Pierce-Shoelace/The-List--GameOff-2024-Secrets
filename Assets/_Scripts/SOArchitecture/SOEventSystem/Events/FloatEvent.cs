using UnityEngine;
namespace RyanPierce.Events
{
    [CreateAssetMenu (fileName = "New Float Event", menuName = "VLGameEvents/Float Event")]
    public class FloatEvent : BaseGameEvent<float>
    {
        public void Raise() => Raise(new float());
    }
}