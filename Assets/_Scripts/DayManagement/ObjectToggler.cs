using System.Collections;
using Shoelace.Audio.XuulSound;
using UnityEngine;

namespace Horror.DayManagement
{
    public class ObjectToggler : MonoBehaviour 
    {
        [SerializeField] private GameObject targetObject;
        [SerializeField] private float enabledDuration = 1f;
        [SerializeField] private float disabledDuration = 0.5f;
        [SerializeField] private bool startEnabled = true;

        [Header("Audio")]
        [SerializeField] private SoundEmitter enableSoundEmitter;
        [SerializeField] private SoundEmitter disableSoundEmitter;

        private void Start()
        {
            if (targetObject == null)
            {
                targetObject = gameObject;
            }

      
            if (enableSoundEmitter == null)
            {
                Debug.LogWarning("Enable sound emitter not assigned on ObjectToggler: " + gameObject.name);
            }
            
            if (disableSoundEmitter == null)
            {
                Debug.LogWarning("Disable sound emitter not assigned on ObjectToggler: " + gameObject.name);
            }

            targetObject.SetActive(startEnabled);
            StartCoroutine(ToggleRoutine());
        }

        private IEnumerator ToggleRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(targetObject.activeSelf ? enabledDuration : disabledDuration);
                bool newState = !targetObject.activeSelf;
                targetObject.SetActive(newState);

                if (newState)
                {
                    enableSoundEmitter?.Play();
                }
                else
                {
                    disableSoundEmitter?.Play();
                }
            }
        }

        public void RestartFlicker()
        {
            StopAllCoroutines();
            StartCoroutine(ToggleRoutine());
        }

        public void StopFlicker()
        {
            StopAllCoroutines();
        }

        public void SetActive(bool state)
        {
            targetObject.SetActive(state);
            
            if (state)
            {
                enableSoundEmitter?.Play();
            }
            else
            {
                disableSoundEmitter?.Play();
            }
        }
    }
}