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
        public override void CallNumber()
        {
            ArrestSequence();
        }

        private async void ArrestSequence()
        {
            AudioManager.Instance.PlayOneShot(arrestSFX);
            await FadeTransition.Instance.ToggleFadeTransition(true, 8f);

            await Task.Delay(8000);

            FadeTransition.Instance.EndScreen();
        }
    }
}
