using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FadeTransition : MonoBehaviour
{
    public static FadeTransition Instance;

    private void Awake()
    {
        if(FadeTransition.Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        cg.alpha = 0f;
    }

    [SerializeField] private CanvasGroup cg;

    public async Task ToggleFadeTransition(bool fadeOut, float duration)
    {
        cg.alpha = fadeOut ? 0f : 1f;
       
        float fade = 0f;
        float t = Time.deltaTime / duration;
        while (fade < 1)
        {
            cg.alpha = Mathf.Lerp(fadeOut ? 0f : 1f, fadeOut ? 1f : 0f, fade);
            fade += t;
            await Task.Yield();
        }

        cg.alpha = fadeOut ? 1f : 0f;
    }
}
