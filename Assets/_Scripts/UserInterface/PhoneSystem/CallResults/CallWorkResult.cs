using DG.Tweening;
using Horror.Chores;
using Horror.InputSystem;
using RyanPierce.Events;
using Shoelace.Audio.XuulSound;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UserInterface;

namespace Horror
{
    public class CallWorkResult : PhoneCallResult
    {
        [SerializeField] private PhoneDialerUI dialer;
        [SerializeField] private InputReader playerInput;
        [SerializeField] private SoundConfig horrorCallSuccess;
        [SerializeField] private GameObject horrorSequenceCanvas;
        [SerializeField] private RawImage callOverlay;
        [SerializeField] private RawImage staticOverlay;
        [SerializeField] private Image disconnectOverlay;

        [SerializeField] private VoidEvent activateToys;
        [SerializeField] private ChoreProgressor chore;
        public override void CallNumber()
        {
            RunWorkCallSequence();
        }

        private async void RunWorkCallSequence()
        {
            horrorSequenceCanvas.SetActive(true);
            dialer.HandleCancel();
            callOverlay.color = new Color(1, 1, 1, 0f);
            playerInput.DisableAllInput();
            AudioManager.Instance.PlayOneShot(horrorCallSuccess);
            callOverlay.DOFade(1f, 9f).SetEase(Ease.InExpo);
            await Task.Delay(9500);
            callOverlay.gameObject.SetActive(false);
            staticOverlay.gameObject.SetActive(true);
            await Task.Delay(500);
            activateToys?.Raise();
            staticOverlay.gameObject.SetActive(false);
            disconnectOverlay.gameObject.SetActive(true);
            await Task.Delay(1500);
            disconnectOverlay.DOFade(0f, 2.5f).SetEase(Ease.InOutSine);
            await Task.Delay(2000);
            disconnectOverlay.gameObject.SetActive(false);
            horrorSequenceCanvas.SetActive(false);

            playerInput.EnableGameplayInput();
            chore?.ProgressChore();
        }
    }
}
