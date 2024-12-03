using DG.Tweening;
using Horror.Chores;
using Horror.InputSystem;
using RyanPierce.Events;
using Shoelace.Audio.XuulSound;
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
            await Awaitable.WaitForSecondsAsync(9.5f);
            callOverlay.gameObject.SetActive(false);
            staticOverlay.gameObject.SetActive(true);
            await Awaitable.WaitForSecondsAsync(.5f);
            activateToys?.Raise();
            staticOverlay.gameObject.SetActive(false);
            disconnectOverlay.gameObject.SetActive(true);
            await Awaitable.WaitForSecondsAsync(1.5f);
            disconnectOverlay.DOFade(0f, 2.5f).SetEase(Ease.InOutSine);
            await Awaitable.WaitForSecondsAsync(2f);
            disconnectOverlay.gameObject.SetActive(false);
            horrorSequenceCanvas.SetActive(false);

            playerInput.EnableGameplayInput();
            chore?.ProgressChore();
        }
    }
}
