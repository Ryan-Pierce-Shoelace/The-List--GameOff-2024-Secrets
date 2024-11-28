using System.Threading.Tasks;
using DG.Tweening;
using Horror.Chores;
using Horror.DayManagement;
using Horror.InputSystem;
using Interaction.InteractionCore;
using Shoelace.Audio.XuulSound;
using UI.Thoughts;
using UnityEngine;
using UnityEngine.UI;

namespace Interaction
{
    public class OvenInteraction : MonoBehaviour, IInteractable
    {
        [SerializeField] private InteractObjectSO ovenObject;
        [SerializeField] private InputReader playerInput;
        [SerializeField] private GameObject highlight;

        public enum OvenState { Closed, Open, Filled, Burning }
        [SerializeField] private OvenState state;

        [SerializeField] private DynamicThoughtSO notReadyToCookThought, burnThought;

        [SerializeField] private SoundConfig openSFX, fillSFX, heatSFX;
        [SerializeField] private GameObject closedOven, openOven, filledOven, hotOven;

        [SerializeField] private SpriteRenderer hotOvenImage;
        [SerializeField] private GameObject smokeOverlayCanvas;
        [SerializeField] private RawImage smokeOverlay;
        [SerializeField] private ParticleSystem smokeParticles;

        [SerializeField] private SpriteRenderer fireAlarm;
        [SerializeField] private Sprite activeFireAlarmSprite;
        [SerializeField] private GameObject fireAlarmLight;
        [SerializeField] private SoundConfig fireAlarmSound;

        [SerializeField] private ChoreProgressor open, fill, burn;
        [SerializeField] private float heatTime;

        [SerializeField] DoorTrigger doorTrigger;

        [SerializeField] private SceneField nextDayScene;

        public void Interact()
        {
            switch (state)
            {
                case OvenState.Closed:
                    open?.ProgressChore();
                    AudioManager.Instance.PlayOneShot(openSFX);
                    state = OvenState.Open;
                    closedOven.gameObject.SetActive(false);
                    openOven.gameObject.SetActive(true);

                    return;
                case OvenState.Open:
                    fill?.ProgressChore();
                    AudioManager.Instance.PlayOneShot(fillSFX);
                    state = OvenState.Filled;
                    filledOven.gameObject.SetActive(true);
                    openOven.gameObject.SetActive(false);
                    return;
                case OvenState.Filled:

                    state = OvenState.Burning;

                    burn?.ProgressChore();
                    AudioManager.Instance.PlayOneShot(heatSFX);
                    

                    filledOven.gameObject.SetActive(false);
                    closedOven.SetActive(true);
                    hotOvenImage.color = new Color(1, 1, 1, 0f);
                    hotOven.gameObject.SetActive(true);

                    RunBurnHorrorSequence();
                    return;
            }
        }

        private async void RunBurnHorrorSequence()
        {
            await Task.Delay(500);
            playerInput.DisableAllInput();
            doorTrigger.SlamDoorway();

            hotOvenImage.DOFade(1f, heatTime);
            await Task.Delay((int)(heatTime * 1000f));
            smokeParticles.Play();
            
            await Task.Delay(7000);
            fireAlarm.sprite = activeFireAlarmSprite;
            fireAlarmLight.SetActive(true);
            smokeOverlayCanvas.gameObject.SetActive(true);
            smokeOverlay.DOFade(1f, 13f);
            AudioManager.Instance.PlayOneShot(fireAlarmSound);
            await Task.Delay(13000);
            FadeTransition.Instance.ChangeDay(nextDayScene, "Day 3");
        }

        public bool IsActive()
        {
            return this.enabled;
        }

        public bool CanInteract()
        {
            if (ovenObject == null)
            {
                Debug.LogError(transform.name + " Has a null Interaction Object");
                return false;
            }

            bool isChoreStateValid = open.GetChoreState() == ChoreState.Available || open.GetChoreState() == ChoreState.Completed;


            if (isChoreStateValid)
            {
                return true;
            }

            if(state != OvenState.Closed)
            {
                return true;
            }


            return false;
        }

        public InteractObjectSO GetInteractableObject()
        {
            return ovenObject;
        }

        public void TriggerFailedInteractionThought()
        {
            notReadyToCookThought?.PlayThought();
        }

        public void TryTriggerSuccessInteractionThought()
        {
            burnThought?.PlayThought();
        }

        public void ToggleHighlight(bool toggle)
        {
            highlight.gameObject.SetActive(toggle);   
        }

        
    }
}