using Horror.InputSystem;
using Interaction.InteractionCore;
using Shoelace.Audio.XuulSound;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Interaction
{
    public class WineSpillInteraction : CleanInteractable
    {
        [SerializeField] private InputReader playerInput;

        [SerializeField] private Animator wineAnim, eyesAnim;

        [SerializeField] private SoundConfig bottleSFX, shatterSFX, mopSFX, horrorSFX;
        private AudioManager audioManager;

        private bool used;

        private TaskCompletionSource<bool> shatterTaskSource;

        protected override void Start()
        {
            base.Start();
            audioManager = AudioManager.Instance;
        }
        public override void Interact()
        {
            if(!used)
            {
                used = true;
                RunSpillSequence();
            }
            else
            {
                base.Interact();
            }
        }



        private async void RunSpillSequence()
        {

        }

        public void PlayShatterSFX()
        {
            audioManager.PlayOneShot(shatterSFX);
        }

        public void ActivateReturnHorror()
        {
            RunBodyHorrorSequence();
        }

        private async void RunBodyHorrorSequence()
        {
            await Task.Delay(1000);
        }

    }
}