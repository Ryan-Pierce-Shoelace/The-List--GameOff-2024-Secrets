using Project.Input;
using Project.Input.InputSystem;
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
			// footstepPlayer = AudioManager.Instance.CreateSound(footStepSound, transform);
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

		// private void UpdateSound()
		// {
		// 	if (rigidBody.velocity.magnitude > 0.1f)
		// 	{
		// 		if (isPlayingFootsteps) return;
		//
		// 		footstepPlayer.Play();
		// 		isPlayingFootsteps = true;
		// 	}
		// 	else if (isPlayingFootsteps)
		// 	{
		// 		footstepPlayer.Stop();
		// 		isPlayingFootsteps = false;
		// 	}
		// }
	}
}