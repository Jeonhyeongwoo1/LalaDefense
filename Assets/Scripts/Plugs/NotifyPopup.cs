using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NotifyPopup : BasePopup
{
    [SerializeField] Transform m_Popup;
    [SerializeField] Text m_Content;
    [SerializeField] Image m_Timer;
    [SerializeField, Range(0, 3)] float m_PopupOpenDuration = 2f;

    [SerializeField] AnimationCurve m_Curve;

    public void SetContent(string content) => m_Content.text = content;

    public override void Open(UnityAction done)
    {
        StartCoroutine(CoUtilize.VLerp((v) => m_Popup.localScale = v, Vector3.zero, Vector3.one, 0.2f, () => Opened(done), m_Curve));
    }

    public override void Close(UnityAction done)
    {
        StartCoroutine(CoUtilize.VLerp((v) => m_Popup.localScale = v, Vector3.one, Vector3.zero, 0.2f, () => Closed(done), m_Curve));
    }

    void Closed(UnityAction done)
    {
        Core.plugs.GetPlugable<Popup>().RemoveOpenedPopup(this);
        done?.Invoke();
        gameObject.SetActive(false);
    }

    void Opened(UnityAction done)
    {
        StartCoroutine(CoUtilize.Lerp((v) => m_Timer.fillAmount = v, 1, 0, m_PopupOpenDuration, () => Close(null)));
        done?.Invoke();
    }

}
