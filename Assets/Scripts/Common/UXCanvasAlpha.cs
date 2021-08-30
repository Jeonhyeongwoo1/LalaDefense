using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UXCanvasAlpha : MonoBehaviour
{
    public bool On
    {
        set
        {
            m_IsOn = value;

            if (value)
            {
                if (!gameObject.activeSelf) { return; }
                StartCoroutine(CanvasAlphaChange());
            }
            else
            {
                StopAllCoroutines();
                canvasGroup.alpha = 1;
            }

        }
    }

    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] bool m_IsOn;
    [SerializeField] float m_Mulitplier;
    [SerializeField] AnimationCurve m_Curve;

    IEnumerator CanvasAlphaChange()
    {
        canvasGroup.alpha = 0;
        while (m_IsOn)
        {
            canvasGroup.alpha = Mathf.PingPong(Time.time * m_Mulitplier, 1);
            yield return null;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        if(m_IsOn)
        {
            StartCoroutine(CanvasAlphaChange());
        }
      
    }

}
