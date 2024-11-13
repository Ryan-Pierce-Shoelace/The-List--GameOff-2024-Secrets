using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.Linq;

namespace Animation.Editor
{
	[CustomEditor(typeof(AnimationEventStateBehaviour))]
	public class AnimationEventStateBehaviourEditor : UnityEditor.Editor
	{
		private Motion previewClip;
		private float previewTime;
		private bool isPreviewing;
		private SpriteRenderer spriteRenderer;

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			AnimationEventStateBehaviour stateBehaviour = (AnimationEventStateBehaviour)target;

			if (Validate(stateBehaviour, out string errorMessage))
			{
				GUILayout.Space(10);

				if (isPreviewing)
				{
					if (GUILayout.Button("Stop Preview"))
					{
						StopPreview();
					}
					else
					{
						PreviewAnimationClip(stateBehaviour);
					}
				}
				else if (GUILayout.Button("Preview"))
				{
					StartPreview();
				}

				GUILayout.Label($"Previewing at {previewTime:F2}s", EditorStyles.helpBox);
			}
			else
			{
				EditorGUILayout.HelpBox(errorMessage, MessageType.Info);
			}
		}

		private void StartPreview()
		{
			isPreviewing = true;
			AnimationMode.StartAnimationMode();
			spriteRenderer = Selection.activeGameObject?.GetComponent<SpriteRenderer>();
		}

		private void StopPreview()
		{
			isPreviewing = false;
			AnimationMode.StopAnimationMode();
		}

		private void PreviewAnimationClip(AnimationEventStateBehaviour stateBehaviour)
		{
			AnimatorController animatorController = GetValidAnimatorController(out string errorMessage);
			if (!animatorController) return;

			ChildAnimatorState matchingState = FindMatchingStateInController(animatorController, stateBehaviour);
			if (!matchingState.state) return;

			Motion motion = matchingState.state.motion;
			if (motion is AnimationClip clip)
			{
				previewTime = stateBehaviour.TriggerTime * clip.length;
				AnimationMode.SampleAnimationClip(Selection.activeGameObject, clip, previewTime);
			}
		}

		private ChildAnimatorState FindMatchingStateInController(AnimatorController controller,
			AnimationEventStateBehaviour behaviour)
		{
			return controller.layers
				.Select(layer => FindMatchingState(layer.stateMachine, behaviour))
				.FirstOrDefault(state => state.state != null);
		}

		private ChildAnimatorState FindMatchingState(AnimatorStateMachine stateMachine,
			AnimationEventStateBehaviour stateBehaviour)
		{
			foreach (var state in stateMachine.states)
			{
				if (state.state.behaviours.Contains(stateBehaviour))
				{
					return state;
				}
			}

			foreach (var subStateMachine in stateMachine.stateMachines)
			{
				var matchingState = FindMatchingState(subStateMachine.stateMachine, stateBehaviour);
				if (matchingState.state != null)
				{
					return matchingState;
				}
			}

			return default;
		}

		private bool Validate(AnimationEventStateBehaviour stateBehaviour, out string errorMessage)
		{
			AnimatorController animatorController = GetValidAnimatorController(out errorMessage);
			if (animatorController == null) return false;

			var matchingState = FindMatchingStateInController(animatorController, stateBehaviour);

			previewClip = matchingState.state?.motion as AnimationClip;
			if (previewClip == null)
			{
				errorMessage = "No valid AnimationClip found for the current state.";
				return false;
			}

			return true;
		}

		private AnimatorController GetValidAnimatorController(out string errorMessage)
		{
			errorMessage = string.Empty;

			GameObject targetGameObject = Selection.activeGameObject;
			if (targetGameObject == null)
			{
				errorMessage = "Please select a GameObject with an Animator to preview.";
				return null;
			}

			Animator animator = targetGameObject.GetComponent<Animator>();
			if (animator == null)
			{
				errorMessage = "The selected GameObject does not have an Animator component.";
				return null;
			}

			AnimatorController animatorController = animator.runtimeAnimatorController as AnimatorController;
			if (animatorController == null)
			{
				errorMessage = "The selected Animator does not have a valid AnimatorController.";
				return null;
			}

			return animatorController;
		}
	}
}