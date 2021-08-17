using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CompletePopup : BasePopup
{
    public Transform popup;

    [SerializeField] Button m_Close;
    [SerializeField] Button m_Home;
    [SerializeField] Button m_Restart;
    [SerializeField] Button m_Next;
    [SerializeField] AnimationCurve m_Curve;

    public override void Open(UnityAction done)
    {
        StartCoroutine(CoUtilize.VLerp((v) => popup.localScale = v, Vector3.zero, Vector3.one, 0.2f, () => Opend(done), m_Curve));
    }

    void Opend(UnityAction done)
    {
        done?.Invoke();
    }

    public override void Close(UnityAction done)
    {
        done?.Invoke();
        gameObject.SetActive(false);
    }

    void OnNext()
    {

    }

    void GoHome()
    {

    }

    void OnRestart()
    {
        StagePlayer.Instance.RestartStage();
        Close(null);
    } 

    // Start is called before the first frame update
    void Start()
    {
        m_Close.onClick.AddListener(() => Close(null));
        m_Next.onClick.AddListener(OnNext);
        m_Home.onClick.AddListener(GoHome);
        m_Restart.onClick.AddListener(OnRestart);
    }

}
