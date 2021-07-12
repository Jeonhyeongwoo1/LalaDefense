using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LoadingAnimation : MonoBehaviour
{
    public Slider slider;
    public RawImage wave;
    public Transform m_Loading;
    public AnimationCurve m_NormalCurve;

    public void ScaleUpText(UnityAction done)
    {
        StartCoroutine(LoadingImageAni(done));
    }

    IEnumerator LoadingTextAni()
    {
        float duration = 3f;
        float elapsed = 0;
        float axisX = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            slider.value = Mathf.Lerp(0, 1, m_NormalCurve.Evaluate(elapsed / duration));
            axisX = Mathf.Lerp(0, elapsed, m_NormalCurve.Evaluate(elapsed / duration));
            wave.uvRect = new Rect(new Vector2(axisX, 0), Vector2.one);
            yield return null;
        }
    }

    IEnumerator LoadingImageAni(UnityAction done)
    {
        float duration = 0.3f;
        float elapsed = 0;
        Vector3 bgScale = m_Loading.localScale;

        while(elapsed < duration)
        {
            elapsed += Time.deltaTime;
            bgScale = Vector3.Lerp(bgScale, Vector3.one, m_NormalCurve.Evaluate(elapsed / duration));
            m_Loading.localScale = bgScale;
            yield return null;
        }

        done?.Invoke();
    }
}
