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

        public void ActivateReturnHorror()
        {
            RunBodyHorrorSequence();
        }

        private async void RunBodyHorrorSequence()
        {
            horrorAnim.gameObject.SetActive(true);
            horrorAnim.Play("WineBodyHorror");
            await Task.Delay(2000);
            AudioManager.Instance.PlayOneShot(horrorSFX);
            playerInput.DisableAllInput();
            await Task.Delay(1000);
            playerInput.EnableGameplayInput();
            await Task.Delay(2000);
            horrorAnim.Play("SpillIdle");
        }
    }
}

