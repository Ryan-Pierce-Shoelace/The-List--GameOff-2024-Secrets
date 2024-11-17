using DG.Tweening;
using Interaction.InteractionCore;
using UI.Thoughts;
using UnityEngine;

namespace Interaction
{
	public class TransformInteractable : BaseObjectInteractable
	{
		[SerializeField] private Transform target;
		[SerializeField] private float transformSpeed = .5f;
		[SerializeField] private Ease transformEasing = Ease.Linear;

		[Header("Cycle Settings")]
		[SerializeField] protected DynamicThoughtSO unDoThoughtSO;

		[SerializeField] private bool cycle = false;

		bool used;

		[SerializeField] private Vector3 newPosition;
		[SerializeField] private Vector3 newEulerRotation;
		[SerializeField] private Vector3 newScale;

		private Vector3 oldPosition;
		private Vector3 oldEulerRotation;
		private Vector3 oldScale;

		protected override void Start()
		{
			base.Start();

			oldPosition = target.localPosition;
			oldEulerRotation = target.localEulerAngles;
			oldScale = target.localScale;

			used = false;
		}

		public override void Interact()
		{
			base.Interact();

			if (!used)
			{
				target.DOLocalMove(newPosition, transformSpeed).SetEase(transformEasing);
				target.DOLocalRotate(newEulerRotation, transformSpeed).SetEase(transformEasing);
				target.DOScale(newScale, transformSpeed).SetEase(transformEasing);

				if (!cycle)
					this.enabled = false;
			}
			else
			{
				target.DOLocalMove(oldPosition, transformSpeed).SetEase(transformEasing);
				target.DOLocalRotate(oldEulerRotation, transformSpeed).SetEase(transformEasing);
				target.DOScale(oldScale, transformSpeed).SetEase(transformEasing);
			}

			used = !used;
		}

		public override void TryTriggerSuccessInteractionThought()
		{
			if (!used)
			{
				base.TryTriggerSuccessInteractionThought();
			}
			else
			{
				if (unDoThoughtSO != null)
				{
					unDoThoughtSO.PlayThought();
				}
			}
		}
	}
}