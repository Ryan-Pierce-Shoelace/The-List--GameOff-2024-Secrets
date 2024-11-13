using UnityEngine;

namespace Animation
{
	public class AnimationEventStateBehaviour : StateMachineBehaviour
	{
		public string EventName;
		[Range(0f, 1f)] public float TriggerTime;

		private bool hasTriggered;
		private AnimationEventReceiver receiver;

		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			hasTriggered = false;
			receiver = animator.GetComponent<AnimationEventReceiver>();
		}

		public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			float currentTime = stateInfo.normalizedTime % 1f;

			if (hasTriggered || !(currentTime >= TriggerTime)) return;

			NotifyReceiver(animator);
			hasTriggered = true;
		}

		private void NotifyReceiver(Animator animator)
		{
			if (receiver != null)
			{
				receiver.OnAnimationEventTriggered(EventName);
			}
		}
	}
}