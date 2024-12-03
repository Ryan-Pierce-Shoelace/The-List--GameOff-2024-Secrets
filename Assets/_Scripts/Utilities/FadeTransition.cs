using Shoelace.Audio.XuulSound;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeTransition : MonoBehaviour
{
    public static FadeTransition Instance;

    private void Awake()
    {
        if(FadeTransition.Instance == null)
        {
            FadeTransition.Instance = this;
            fadeOutCG.alpha = 0f;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [SerializeField] private CanvasGroup fadeOutCG;

    public async Task ToggleFadeTransition(bool fadeOut, float duration)
    {
        await FadeCanvasGroup(fadeOutCG, fadeOut, duration);
    }

    private async Awaitable FadeCanvasGroup(CanvasGroup target, bool fadeOut, float duration)
    {
        target.alpha = fadeOut ? 0f : 1f;

        float fade = 0f;
        float t = Time.deltaTime / duration;
        while (fade < 1)
        {
            target.alpha = Mathf.Lerp(fadeOut ? 0f : 1f, fadeOut ? 1f : 0f, fade);
            fade += t;
            await Awaitable.NextFrameAsync();
        }

        target.alpha = fadeOut ? 1f : 0f;
    }

    [SerializeField] private CanvasGroup endScreenCG;
    [SerializeField] private SceneField mainMenu;
    [SerializeField] private SoundConfig endMusic;

    public async void EndScreen()
    {
        AudioManager.Instance.PlayMusic(endMusic);
        await FadeCanvasGroup(endScreenCG, true, 2f);
        fadeOutCG.alpha = 0f;
        await Awaitable.WaitForSecondsAsync(1f);
        SceneManager.LoadScene(mainMenu);
        await FadeCanvasGroup(endScreenCG, false, 1f);
    }

    [SerializeField] private CanvasGroup dayChangeCG;
    [SerializeField] private TMP_Text dayChangeText;

    public void ChangeDay(SceneField newScene, string dayChangeText)
    {
        HandleDayChange(newScene, dayChangeText);
    }
    private async void HandleDayChange(SceneField scene, string displayText)
    {

        dayChangeText.text = displayText;
        await FadeCanvasGroup(dayChangeCG, true, 3f);
        AsyncOperation load = SceneManager.LoadSceneAsync(scene);
        load.allowSceneActivation = false;

        while(!load.isDone)
        {
            if(load.progress >= .9f)
                load.allowSceneActivation =true;

            await Awaitable.NextFrameAsync();
        }

        await FadeCanvasGroup(dayChangeCG, false, 3f);
    }
}
