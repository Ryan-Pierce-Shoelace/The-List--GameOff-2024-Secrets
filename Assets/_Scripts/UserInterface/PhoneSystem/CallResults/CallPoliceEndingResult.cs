using Horror.InputSystem;
using Shoelace.Audio.XuulSound;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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

            await Task.Delay(8000);
            input.EnableUIInput();
            FadeTransition.Instance.EndScreen();
        }
    }
}
