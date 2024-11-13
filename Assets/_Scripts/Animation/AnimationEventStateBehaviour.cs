using UnityEngine;

namespace Animation
{
	public class AnimationEventStateBehaviour : StateMachineBehaviour
	{
		public string EventName;
		[Range(0f, 1f)] public float TriggerTime;

		[Tooltip("If true, event will trigger each time animation loops. If false, triggers only once.")]
		public bool RepeatOnLoop = true;

		private bool hasTriggered;
		private int lastPlayedLoop = -1;
		private AnimationEventReceiver receiver;


		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			hasTriggered = false;
			lastPlayedLoop = -1;
			receiver = animator.GetComponent<AnimationEventReceiver>();
		}

		public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			float currentTime = stateInfo.normalizedTime % 1f;
			int currentLoop = Mathf.FloorToInt(stateInfo.normalizedTime);

			bool shouldTrigger = currentTime >= TriggerTime &&
			                     (RepeatOnLoop ? currentLoop != lastPlayedLoop : !hasTriggered);

			if (!shouldTrigger) return;


			NotifyReceiver(animator);
			lastPlayedLoop = currentLoop;
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