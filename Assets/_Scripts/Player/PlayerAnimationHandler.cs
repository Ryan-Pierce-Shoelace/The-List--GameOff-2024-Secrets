using UnityEngine;
namespace Horror.Player
{
    public class PlayerAnimationHandler : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody;
        private SpriteRenderer spriteRenderer;
        private Animator _animator;
        private bool movingLeft;
        private bool isMoving;
        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
            movingLeft = false;
        }

        private void Update()
        {
            if (_rigidbody.velocity.x > 0)
                movingLeft = false;
            if(_rigidbody.velocity.x < 0)
                movingLeft = true;

            spriteRenderer.flipX = movingLeft;
            isMoving = _rigidbody.velocity.magnitude > 0;
        }

        private void LateUpdate()
        {
            _animator.SetBool("IsMoving", isMoving);
        }
    }
}
