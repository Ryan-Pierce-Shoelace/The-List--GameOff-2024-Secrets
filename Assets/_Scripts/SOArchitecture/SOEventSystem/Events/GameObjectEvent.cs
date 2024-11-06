using UnityEngine;
namespace RyanPierce.Events
{
    /// <summary>
    /// Note that due to protection levels you can't make a transform event, so in those cases use this instead then just get the transform afterwards
    /// </summary>
    [CreateAssetMenu (fileName = "New GameObject Event", menuName = "VLGameEvents/GameObject Event")]
    public class GameObjectEvent : BaseGameEvent<GameObject>
    {
        public void Raise() => Raise(new GameObject());
    }
}