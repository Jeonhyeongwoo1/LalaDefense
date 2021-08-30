using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BlockSkybox : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] AnimationCurve curve;

    public void SetAlpha(float alpha)
    {
        canvasGroup.alpha = alpha;
    }

    public void FadeOut(float duration = 1, UnityAction done = null)
    {
        if (canvasGroup.alpha == 0)
        {
            done?.Invoke();
            canvasGroup.gameObject.SetActive(false);
            return;
        }

        StartCoroutine(CoUtilize.Lerp((v) => canvasGroup.alpha = v, 1, 0, duration, () =>
        {
            done?.Invoke();
            canvasGroup.gameObject.SetActive(false);
        }, curve));
    }

    public void FadeIn(float duration = 1, UnityAction done = null)
    {
        if (canvasGroup.alpha == 1)
        {
            done?.Invoke();
            return;
        }

        canvasGroup.gameObject.SetActive(true);
        StartCoroutine(CoUtilize.Lerp((v) => canvasGroup.alpha = v, 0, 1, duration, done, curve));
    }

    public void Fade(float duration = 1, UnityAction done = null)
    {
        FadeIn(duration, () => FadeOut(duration,
                                () =>
                                    {
                                        done?.Invoke();
                                        canvasGroup.gameObject.SetActive(false);
                                    }));
    }

}
