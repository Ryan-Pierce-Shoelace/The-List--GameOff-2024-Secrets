using Horror;
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
    public class WineSpillInteraction : BaseObjectInteractable
    {
        [SerializeField] private Animator wineAnim, eyesAnim;
        [SerializeField] private ChoreRevealer postWineShatterRevealer;
        [SerializeField] private SoundConfig shatterSFX;
        private AudioManager audioManager;

        [SerializeField] private GameObject spillGameObject;

        protected override void Start()
        {
            base.Start();
            audioManager = AudioManager.Instance;
        }
        public override void Interact()
        {
            RunSpillSequence();
        }

        private async void RunSpillSequence()
        {
            wineAnim.Play("WineSpill");
            eyesAnim.Play("CreepyEyes");
            await Task.Delay(4000);
            postWineShatterRevealer.TryRevealNewChores();
            
            spillGameObject.SetActive(true);

            ToggleHighlight(false);
            gameObject.SetActive(false);
            this.enabled = false;
        }

        public void PlayShatterSFX()
        {
            audioManager.PlayOneShot(shatterSFX);
        }

        

    }
}