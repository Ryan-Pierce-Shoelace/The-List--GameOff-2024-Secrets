using DG.Tweening;
using Horror;
using Horror.DayManagement;
using Horror.InputSystem;
using Horror.RoomNavigation;
using Shoelace.Audio.XuulSound;
using System.Threading.Tasks;
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

    private TaskCompletionSource<bool> toyAnimCompletion;
    public async void RunHorrorSequence()
    {
        await Task.Delay(300);
        kidsRoomDoor.CloseDoorway();
        await Task.Delay(2000);
        playerInput.DisableAllInput();
        kidsRoomOverlay.DOFade(1f, 6f);
        kidToyAnim.SetTrigger("Horror");

        toyAnimCompletion = new TaskCompletionSource<bool>();

        await toyAnimCompletion.Task;

        AudioManager.Instance.PlayOneShot(horrorNoise);

        await Task.Delay(100);
        halldoorTrigger.SlamDoorway();
        await Task.Delay(100);
        
        navigator.ForceMoveToNewRoom(escapeDoor);

        kidsRoomOverlay.DOFade(0f, 1f).OnComplete(() => kidsRoomOverlay.gameObject.SetActive(false));
        revealer.TryRevealNewChores();
    }

    public void AnimationComplete()
    {
        toyAnimCompletion?.TrySetResult(true);
    }
}
