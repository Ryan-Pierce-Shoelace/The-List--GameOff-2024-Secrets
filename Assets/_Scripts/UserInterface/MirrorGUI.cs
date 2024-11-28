using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Shoelace.Audio.XuulSound;

namespace UserInterface
{
    public class MirrorGUI : UIScreen
    {
        [SerializeField] private bool lockInForHorror;
        [SerializeField] private Button exitbutton;
        [SerializeField] private RawImage overlayEffect;
        [SerializeField] private Animator horrorAnimation;

        [SerializeField] private SoundConfig SliceSFX;

        [SerializeField] private SceneField nextDay;
        protected override void OnEnable()
        {
            overlayEffect.color = Color.clear;
            if(lockInForHorror)
            {
                RunMirrorSequence();
            }
            else
            {
                horrorAnimation.Play("MirrorNormal");
                base.OnEnable();
            }
        }

        private async void RunMirrorSequence()
        {
            inputReader.DisableAllInput();
            exitbutton.interactable = false;

            overlayEffect.DOColor(Color.white, 9f);
            await Task.Delay(5000);
            horrorAnimation.Play("HorrorMirror");
            await Task.Delay(4000);
            overlayEffect.color = Color.white;
            FadeTransition.Instance.ChangeDay(nextDay, "Day 2");
        }

        public void SliceThroat()
        {
            AudioManager.Instance.PlayOneShot(SliceSFX);
        }

    }
}