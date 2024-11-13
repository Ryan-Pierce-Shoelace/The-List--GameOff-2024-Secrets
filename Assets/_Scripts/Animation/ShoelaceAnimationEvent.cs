using System;
using UnityEngine.Events;

namespace Animation
{
	[Serializable]
	public class ShoelaceAnimationEvent {
		public string EventName;
		public UnityEvent OnAnimationEvent;
	}
}