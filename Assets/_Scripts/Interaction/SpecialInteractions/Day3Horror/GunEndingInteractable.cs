using Horror.InputSystem;
using Interaction.InteractionCore;
using Shoelace.Audio.XuulSound;
using System.Threading.Tasks;
using UnityEngine;

public class GunEndingInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private InteractObjectSO gunObject;
    [SerializeField] private GameObject highlight;
    [SerializeField] private SoundConfig gunShotSFX;
    [SerializeField] private InputReader input;
    private void Start()
    {
        ToggleHighlight(false);
    }

    public async void Interact()
    {
        input.DisableAllInput();
        await FadeTransition.Instance.ToggleFadeTransition(true, 4f);
        AudioManager.Instance.PlayOneShot(gunShotSFX);
        await Task.Delay(7000);
        input.EnableUIInput();
        FadeTransition.Instance.EndScreen();
    }


    public bool CanInteract()
    {
        return true;
    }

    public InteractObjectSO GetInteractableObject()
    {
        return gunObject;
    }

    

    public bool IsActive()
    {
        return enabled;
    }

    public void ToggleHighlight(bool toggle)
    {
        highlight.SetActive(toggle);
    }

    public void TriggerFailedInteractionThought()
    {
    }

    public void TryTriggerSuccessInteractionThought()
    {
    }
}
