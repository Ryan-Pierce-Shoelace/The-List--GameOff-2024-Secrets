using Project.Input;
using UnityEngine;

namespace Horror.Game
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private InputReader input;
        [SerializeField] private float moveSpeed;

        private Rigidbody2D _rigidBody;
        private void Start()
        {
            _rigidBody = GetComponent<Rigidbody2D>();
        }


        private void FixedUpdate()
        {
            _rigidBody.velocity = input.currentMove * moveSpeed;
        }


    }
}
