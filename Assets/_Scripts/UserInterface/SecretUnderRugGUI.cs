using Shoelace.Audio.XuulSound;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UserInterface
{
    public class SecretUnderRugGUI : UIScreen
    {
        [SerializeField] private SoundConfig openHorrorSFX;
        protected override void OnEnable()
        {
            if(openHorrorSFX != null)
                AudioManager.Instance.PlayOneShot(openHorrorSFX);

            base.OnEnable();
        }
    }
}
