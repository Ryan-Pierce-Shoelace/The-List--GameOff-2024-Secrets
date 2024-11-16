using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace UserInterface
{
    public class MirrorGUI : UIScreen
    {
        [SerializeField] private bool lockInForHorror;
        [SerializeField] private Button exitbutton;
        [SerializeField] private RawImage overlayEffect;

        protected override void OnEnable()
        {
            overlayEffect.color = Color.clear;
            if(lockInForHorror)
            {
                RunMirrorSequence();
            }
            else
            {
                base.OnEnable();
            }
        }

        private async void RunMirrorSequence()
        {
            inputReader.DisableAllInput();
            exitbutton.interactable = false;

            overlayEffect.DOColor(Color.white, 5f);
            await Task.Delay(5000);
            overlayEffect.color = Color.white;
            await Task.Delay(5000);

            FinishHorrorSequence();
        }

        private void FinishHorrorSequence()
        {
            overlayEffect.color = Color.clear;
            exitbutton.interactable = true; 

            overlayEffect.gameObject.SetActive(false);

            inputReader.EnableUIInput();
            inputReader.CancelEvent += HandleCancel;
        }
    }
}