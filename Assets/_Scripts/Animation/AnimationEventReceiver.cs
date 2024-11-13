using System.Collections.Generic;
using UnityEngine;

namespace Animation
{
	public class AnimationEventReceiver : MonoBehaviour
	{
		[SerializeField]
		private List<ShoelaceAnimationEvent> animationEvents = new();

		public void OnAnimationEventTriggered(string eventName)
		{
			ShoelaceAnimationEvent matchingEvent = animationEvents.Find(se => se.EventName == eventName);
			matchingEvent?.OnAnimationEvent?.Invoke();
		}
	}
}