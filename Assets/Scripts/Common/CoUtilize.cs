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
