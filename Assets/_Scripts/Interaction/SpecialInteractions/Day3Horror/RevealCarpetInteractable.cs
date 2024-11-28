using Horror.Chores;
using Interaction.InteractionCore;
using RyanPierce.Events;
using Shoelace.Audio.XuulSound;
using UI.Thoughts;
using UnityEngine;

public class RevealCarpetInteractable : MonoBehaviour, IInteractable
{
    [Header("Rug Info")]
    [SerializeField] private InteractObjectSO rugObject;
    [SerializeField] private GameObject rugHighlight;
    [SerializeField] private DynamicThoughtSO rugUseThought;
    [SerializeField] private SoundConfig rugUseSFX;

    [SerializeField] private SpriteRenderer rugRenderer;
    [SerializeField] private Sprite revealedDoorSprite;

    private bool rugRevealed;
    [Header("Trap Door Info")]
    [SerializeField] private InteractObjectSO trapDoorObject;
    [SerializeField] private GameObject trapDoorHighlight;
    [SerializeField] private SoundConfig trapDoorUseSFX;

    [SerializeField] private VoidEvent openSecretGUIEvent;

    [Header("Chore Data")]
    [SerializeField] private ChoreProgressor findKnifeChore;

    public bool CanInteract()
    {
        if(rugRevealed)
        {
            if (trapDoorObject == null)
            {
                Debug.LogError(transform.name + " Has a null Interaction Object");
                return false;
            }
        }
        else
        {
            if (rugObject == null)
            {
                Debug.LogError(transform.name + " Has a null Interaction Object");
                return false;
            }
        }
        

        if (findKnifeChore == null) return false;

        bool isChoreStateValid = findKnifeChore.GetChoreState() == ChoreState.Available;


        if (isChoreStateValid)
        {
            return true;
        }

        return false;
    }

    public InteractObjectSO GetInteractableObject()
    {
        return rugRevealed ? trapDoorObject : rugObject;
    }

    public void Interact()
    {
        if(rugRevealed)
        {
            //Open Trap Door
            AudioManager.Instance.PlayOneShot(trapDoorUseSFX);
            openSecretGUIEvent?.Raise();
        }
        else
        {
            //Uncover Rug
            rugRevealed = true;
            rugRenderer.sprite = revealedDoorSprite;
            AudioManager.Instance.PlayOneShot(rugUseSFX);

        }
    }

    public bool IsActive()
    {
        return enabled;
    }

    public void ToggleHighlight(bool toggle)
    {
        if (rugRevealed)
        {
            trapDoorHighlight.gameObject.SetActive(toggle);
            rugHighlight.gameObject.SetActive(false);
        }
        else
        {
            trapDoorHighlight.gameObject.SetActive(false);
            rugHighlight.gameObject.SetActive(toggle);
        }
    }

    public void TriggerFailedInteractionThought()
    {
    }
    public void TryTriggerSuccessInteractionThought()
    {
        if (!rugRevealed)
            rugUseThought?.PlayThought();
    }
}
