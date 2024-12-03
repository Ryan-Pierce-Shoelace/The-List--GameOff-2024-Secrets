using DG.Tweening;
using Horror;
using Horror.InputSystem;
using Shoelace.Audio.XuulSound;
using UnityEngine;
using UnityEngine.UI;

public class HallwayRoomReveal : MonoBehaviour
{
    [SerializeField] private InputReader playerInput;
    [SerializeField] private GameObject normalHallway;
    [SerializeField] private GameObject revealHallway;
    [SerializeField] private int revealFlicker;

    [SerializeField] private RawImage hallwayRevealOverlay;
    [SerializeField] private SoundConfig suspenseSFX;

    [SerializeField] private ChoreRevealer revealChore;

    public async void RunRevealSequence()
    {
        playerInput.DisableAllInput();
       
        hallwayRevealOverlay.gameObject.SetActive(true);
        hallwayRevealOverlay.DOFade(1f, 5f);
        await Awaitable.WaitForSecondsAsync(3f);

        for (int i = 0; i < revealFlicker; i++)
        {
            normalHallway.SetActive(!normalHallway.activeInHierarchy);
            revealHallway.SetActive(!revealHallway.activeInHierarchy);
            AudioManager.Instance.PlayOneShot(suspenseSFX);
            await Awaitable.WaitForSecondsAsync(.25f);
        }

        normalHallway.SetActive(false);
        revealHallway.SetActive(true);
        hallwayRevealOverlay.DOKill();

        hallwayRevealOverlay.DOFade(0f, 1f);
        await Awaitable.WaitForSecondsAsync(1f);
        hallwayRevealOverlay.gameObject.SetActive(false);
        playerInput.EnableGameplayInput();
        revealChore.TryRevealNewChores();
    }
}
