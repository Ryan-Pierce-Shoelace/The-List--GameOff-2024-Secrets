using Horror.InputSystem;
using Shoelace.Audio.XuulSound;
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


		private void Start()
		{
			rigidBody = GetComponent<Rigidbody2D>();
		}

		private void OnDestroy()
		{
			footstepPlayer?.Dispose();
		}

		private void FixedUpdate()
		{
			rigidBody.velocity = input.CurrentMove * moveSpeed;
		}

		public void PlaySound()
		{
			AudioManager.Instance.PlayOneShot(footStepSound);
		}
	}
}