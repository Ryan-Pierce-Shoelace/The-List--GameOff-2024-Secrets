using Horror;
using Interaction.InteractionCore;
using Shoelace.Audio.XuulSound;
using UnityEngine;

namespace Interaction
{
    public class WineSpillInteraction : BaseObjectInteractable
    {
        [SerializeField] private Animator wineAnim, eyesAnim;
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

            await Awaitable.WaitForSecondsAsync(1f);

            ToggleHighlight(false);
            gameObject.SetActive(false);
            this.enabled = false;
            choreProgressor?.ProgressChore();
            spillGameObject.SetActive(true);
            
        }

        public void PlayShatterSFX()
        {
            audioManager.PlayOneShot(shatterSFX);
        }

        

    }
}