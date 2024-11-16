using DG.Tweening;
using Interaction.InteractionCore;
using UnityEngine;
namespace Interaction
{
    public class TransformInteractable : BaseObjectInteractable
    {
        [SerializeField] private Transform target;

        [SerializeField] private Vector3 newPosition;
        [SerializeField] private Vector3 newEulerRotation;
        [SerializeField] private Vector3 newScale;


        public override void Interact()
        {
            base.Interact();

            target.DOLocalMove(newPosition, .5f);
            target.DOLocalRotate(newEulerRotation, .5f);
            target.DOScale(newScale, .5f);

            this.enabled = false;
        }
    }
}