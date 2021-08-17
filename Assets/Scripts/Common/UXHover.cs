using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UXHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public bool OnHover
    {
        set
        {
            m_IsOn = value;
            if (m_IsOn)
            {
                OnPointerEnter(null);
            }
        }
    }

    [SerializeField] bool m_IsOn = true;
    [SerializeField] Vector3 m_Hover = new Vector3(1.1f, 1.1f, 1);
    [SerializeField] float m_Duration = 0.2f;
    [SerializeField] AnimationCurve m_Curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] Transform m_Target;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!m_IsOn) { return; }
        StopAllCoroutines();
        StartCoroutine(CoUtilize.VLerp((v) => m_Target.localScale = v, m_Target.localScale, m_Hover, m_Duration, null, m_Curve));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!m_IsOn) { return; }
        StopAllCoroutines();
        StartCoroutine(CoUtilize.VLerp((v) => m_Target.localScale = v, m_Target.localScale, Vector3.one, m_Duration, null, m_Curve));
    }

    // Start is called before the first frame update
    void Start()
    {
        if (m_Target == null)
        {
            m_Target = GetComponent<Transform>();
        }
    }

}
