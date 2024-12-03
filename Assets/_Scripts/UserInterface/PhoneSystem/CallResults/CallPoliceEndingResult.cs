using Horror.InputSystem;
using Shoelace.Audio.XuulSound;
using UnityEngine;

namespace Horror
{
    public class CallPoliceEndingResult : PhoneCallResult
    {
        [SerializeField] private SoundConfig arrestSFX;
        [SerializeField] private InputReader input;
        public override void CallNumber()
        {
            input.DisableAllInput();
            ArrestSequence();
        }

        private async void ArrestSequence()
        {
            AudioManager.Instance.PlayOneShot(arrestSFX);
            await FadeTransition.Instance.ToggleFadeTransition(true, 8f);

            await Awaitable.WaitForSecondsAsync(8f);
            input.EnableUIInput();
            FadeTransition.Instance.EndScreen();
        }
    }
}
