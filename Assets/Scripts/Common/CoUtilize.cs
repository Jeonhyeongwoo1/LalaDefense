using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class CoUtilize
{

    public static IEnumerator VLerp(UnityAction<Vector3> call, Vector3 s, Vector3 e, float duration, UnityAction done = null, AnimationCurve c = null)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            call.Invoke(Vector3.Lerp(s, e, c != null ? c.Evaluate(elapsed / duration) : elapsed / duration));
            yield return null;
        }

        done?.Invoke();
    }

    public static IEnumerator Lerp(UnityAction<float> call, float s, float e, float duration, UnityAction done = null, AnimationCurve c = null)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            call.Invoke(Mathf.Lerp(s, e, c != null ? c.Evaluate(elapsed / duration) : elapsed / duration));
            yield return null;
        }

        done?.Invoke();
    }

    public static IEnumerator Shaking(UnityAction<Vector3> call, float shakeRange, float duration, UnityAction done = null)
    {
        Vector3 value = Vector3.zero;
        float elapsed = 0;
        while(elapsed < duration)
        {
            elapsed += Time.deltaTime;
            value = new Vector3(Random.Range(-shakeRange, shakeRange), 
                                Random.Range(-shakeRange, shakeRange), 
                                Random.Range(-shakeRange, shakeRange));
            call.Invoke(value);
            yield return null;
        }

        done?.Invoke();
    }

    public static IEnumerator VLerpUnclamped(UnityAction<Vector3> call, Vector3 s, Vector3 e, float duration, UnityAction done = null, AnimationCurve c = null)
    {
        float elapsed = 0f;
        while(elapsed < duration)
        {
            elapsed += Time.deltaTime;
            call.Invoke(Vector3.LerpUnclamped(s, e, c != null ? c.Evaluate(elapsed / duration) : elapsed / duration));
            yield return null;
        }

        done?.Invoke();
    }

}
