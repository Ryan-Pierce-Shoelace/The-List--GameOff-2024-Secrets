using DG.Tweening;
using Horror;
using Horror.DayManagement;
using Horror.InputSystem;
using Horror.RoomNavigation;
using Shoelace.Audio.XuulSound;
using UnityEngine;
using UnityEngine.UI;

public class KidRoomHorror : MonoBehaviour
{
    [SerializeField] private InputReader playerInput;
    [SerializeField] private RawImage kidsRoomOverlay;
    [SerializeField] private Animator kidToyAnim;
    [SerializeField] private SoundConfig horrorNoise;

    [SerializeField] private PlayerNavigator navigator;
    [SerializeField] private Doorway escapeDoor;
    [SerializeField] private DoorTrigger kidsRoomDoor;
    [SerializeField] private DoorTrigger halldoorTrigger;

    [SerializeField] private ChoreRevealer revealer;

    private AwaitableCompletionSource<bool> toyAnimCompletion;
    public async void RunHorrorSequence()
    {
        await Awaitable.WaitForSecondsAsync(.3f);
        kidsRoomDoor.CloseDoorway();
        await Awaitable.WaitForSecondsAsync(2f);
        playerInput.DisableAllInput();
        kidsRoomOverlay.DOFade(1f, 6f);
        kidToyAnim.SetTrigger("Horror");

        toyAnimCompletion = new AwaitableCompletionSource<bool>();

        await toyAnimCompletion.Awaitable;

        AudioManager.Instance.PlayOneShot(horrorNoise);

        await Awaitable.WaitForSecondsAsync(.1f);
        halldoorTrigger.SlamDoorway();
        await Awaitable.WaitForSecondsAsync(.1f);

        navigator.ForceMoveToNewRoom(escapeDoor);

        kidsRoomOverlay.DOFade(0f, 1f).OnComplete(() => kidsRoomOverlay.gameObject.SetActive(false));
        revealer.TryRevealNewChores();
    }

    public void AnimationComplete()
    {
        toyAnimCompletion?.TrySetResult(true);
    }
}
