using Shoelace.Audio.XuulSound;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UserInterface;
using Horror.Chores;
using UI.Thoughts;

namespace Horror
{
    public class KnifeBlockGUI : UIScreen
    {
        [SerializeField] private RawImage overlayEffect;
        [SerializeField] private Animator guiAnimation;

        [SerializeField] private SoundConfig suspenseSFX;

        [SerializeField] private ChoreProgressor progressor;
        [SerializeField] private DynamicThoughtSO whereIsItThought;

        protected override void OnEnable()
        {
            overlayEffect.color = new Color(1f, 1f, 1f, 0f);
            RunKnifeBlockSequence();

            guiAnimation.gameObject.SetActive(false);
        }

        private async void RunKnifeBlockSequence()
        {
            inputReader.DisableAllInput();

            await FadeTransition.Instance.ToggleFadeTransition(true, .5f);
            await FadeTransition.Instance.ToggleFadeTransition(false, .5f);

            guiAnimation.gameObject.SetActive(true);
            guiAnimation.Play("Horror");

            await Awaitable.WaitForSecondsAsync(3f);
            overlayEffect.DOColor(Color.white, 1f);
            AudioManager.Instance.PlayOneShot(suspenseSFX);
            await Awaitable.WaitForSecondsAsync(1f);
            overlayEffect.color = Color.white;

            await FadeTransition.Instance.ToggleFadeTransition(true, .5f);
            await FadeTransition.Instance.ToggleFadeTransition(false, .5f);

            FinishHorrorSequence();
        }

        private void FinishHorrorSequence()
        {
            progressor?.ProgressChore();
            overlayEffect.color = Color.clear;
            whereIsItThought?.PlayThought();
            HandleCancel();
        }
    }
}
