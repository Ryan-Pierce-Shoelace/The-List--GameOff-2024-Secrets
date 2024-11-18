using Horror.InputSystem;
using Shoelace.Audio.XuulSound;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
namespace Interaction
{
    public class HorrorSpillClean : CleanInteractable
    {
        [SerializeField] private InputReader playerInput;

        [SerializeField] private Animator horrorAnim;
        [SerializeField] private SoundConfig horrorSFX;

        private TaskCompletionSource<bool> horrorHitSource;

        public void ActivateReturnHorror()
        {
            RunBodyHorrorSequence();
        }

        private async void RunBodyHorrorSequence()
        {
            horrorAnim.gameObject.SetActive(true);
            horrorAnim.Play("WineBodyHorror");

            horrorHitSource = new TaskCompletionSource<bool>();

            await horrorHitSource.Task;


            await Task.Delay(3000);
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

