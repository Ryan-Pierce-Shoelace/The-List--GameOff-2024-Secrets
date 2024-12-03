using Horror.InputSystem;
using Shoelace.Audio.XuulSound;
using UnityEngine;
namespace Interaction
{
    public class HorrorSpillClean : CleanInteractable
    {
        [SerializeField] private InputReader playerInput;

        [SerializeField] private Animator horrorAnim;
        [SerializeField] private SoundConfig horrorSFX;

        private AwaitableCompletionSource<bool> horrorHitSource;

        public void ActivateReturnHorror()
        {
            RunBodyHorrorSequence();
        }

        private async void RunBodyHorrorSequence()
        {
            horrorAnim.gameObject.SetActive(true);
            horrorAnim.Play("WineBodyHorror");

            horrorHitSource = new AwaitableCompletionSource<bool>();

            await horrorHitSource.Awaitable;


            await Awaitable.WaitForSecondsAsync(3f);
            playerInput.EnableGameplayInput();
            horrorAnim.Play("SpillIdle");
        }

        public void ActivateHorrorHit()
        {
            AudioManager.Instance.PlayOneShot(horrorSFX);
            playerInput.DisableAllInput();
            horrorHitSource?.TrySetResult(true);
        }
    }
}

