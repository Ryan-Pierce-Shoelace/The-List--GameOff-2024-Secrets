using Horror.InputSystem;
using Shoelace.Audio.XuulSound;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Horror.Player
{
	public class PlayerMovement : MonoBehaviour
	{
		[SerializeField] private SoundConfig footStepSound;

		[SerializeField] private InputReader input;
		[SerializeField] private float moveSpeed;

		private Rigidbody2D rigidBody;
		private ISoundPlayer footstepPlayer;
		private float lastFootstepTime;

		private bool inputEnabled;

		private void Start()
		{
			ToggleInput(true);
			rigidBody = GetComponent<Rigidbody2D>();
		}

		private void OnDestroy()
		{
			footstepPlayer?.Dispose();
		}

		private void FixedUpdate()
		{
			if(inputEnabled)
				rigidBody.velocity = input.CurrentMove * moveSpeed;
		}

		public void PlaySound()
		{
			AudioManager.Instance.PlayOneShot(footStepSound);
		}

        public async Task AutoWalkToPosition(Vector2 doorExitPos)
        {
            while (Vector2.Distance(rigidBody.position, doorExitPos) > .1f)
			{
				Vector2 dir = (doorExitPos - rigidBody.position).normalized;
				rigidBody.velocity = dir * moveSpeed;

				await Task.Yield();
			}

			rigidBody.velocity = Vector2.zero;
        }

        public void ToggleInput(bool toggle)
        {
            inputEnabled = toggle;
        }
    }
}