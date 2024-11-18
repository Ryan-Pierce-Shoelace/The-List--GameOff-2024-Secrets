using Interaction.InteractionCore;
using Shoelace.Audio.XuulSound;
using UI.Thoughts;
using UnityEngine;

namespace Interaction
{
	public class ToggleInteractable : BaseObjectInteractable
	{
		[SerializeField] private GameObject target;
		[SerializeField] private bool cycle;

		[SerializeField] private SoundConfig unToggleSFX;

		[Header("Cycle Settings")]
		[SerializeField] protected DynamicThoughtSO unDoThoughtSO;


		public override void Interact()
		{
			base.Interact();
			target.SetActive(!target.activeInHierarchy);

			if (cycle == false)
			{
				this.enabled = false;
			}
		}

		protected override void TryPlaySFX()
		{
			if (target.activeInHierarchy)
			{
				base.TryPlaySFX();
			}
			else
			{
				if (unToggleSFX != null)
				{
					AudioManager.Instance.PlayOneShot(unToggleSFX);
				}
			}
		}

		public override void TryTriggerSuccessInteractionThought()
		{
			if (target.activeInHierarchy)
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