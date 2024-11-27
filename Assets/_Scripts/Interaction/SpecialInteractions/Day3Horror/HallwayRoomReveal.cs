using DG.Tweening;
using Horror;
using Horror.InputSystem;
using Shoelace.Audio.XuulSound;
using System.Threading.Tasks;
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
    [SerializeField] private SoundConfig revealSFX;

    [SerializeField] private ChoreRevealer revealChore;

    public async void RunRevealSequence()
    {
        playerInput.DisableAllInput();
       
        hallwayRevealOverlay.gameObject.SetActive(true);
        hallwayRevealOverlay.DOFade(1f, 5f);
        await Task.Delay(3000);

        for (int i = 0; i < revealFlicker; i++)
        {
            normalHallway.SetActive(!normalHallway.activeInHierarchy);
            revealHallway.SetActive(!revealHallway.activeInHierarchy);
            AudioManager.Instance.PlayOneShot(suspenseSFX);
            await Task.Delay(250);
        }

        normalHallway.SetActive(false);
        revealHallway.SetActive(true);
        hallwayRevealOverlay.DOKill();

        hallwayRevealOverlay.DOFade(0f, 1f);
        AudioManager.Instance.PlayOneShot(revealSFX);
        await Task.Delay(1000);
        hallwayRevealOverlay.gameObject.SetActive(false);
        playerInput.EnableGameplayInput();
        revealChore.TryRevealNewChores();
    }
}
