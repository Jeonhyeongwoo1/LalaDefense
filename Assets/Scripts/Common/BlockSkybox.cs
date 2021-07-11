using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BlockSkybox : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] AnimationCurve curve;

    public void FadeOut(float duration = 1, UnityAction done = null)
    {
        if (!gameObject.activeSelf) { return; }
        StartCoroutine(Fade(false, duration, () => { done?.Invoke(); gameObject.SetActive(false); }));
    }

    public void FadeIn(float duration = 1, UnityAction done = null)
    {
        gameObject.SetActive(true);
        StartCoroutine(Fade(true, duration, done));
    }

    IEnumerator Fade(bool isFadeIn, float duration, UnityAction done)
    {
        float start = isFadeIn ? 0 : 1;
        float end = isFadeIn ? 1 : 0;
        float elapsed = 0;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, end, curve.Evaluate(elapsed / duration));
            yield return null;
        }

        done?.Invoke();
    }

}
