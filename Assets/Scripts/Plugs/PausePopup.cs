using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PausePopup : BasePopup
{
    public Transform pause;
    public GameObject m_QuitPopup;

    [SerializeField] Button m_QuitCancel;
    [SerializeField] Button m_QuitOk;
    [SerializeField] Button m_Quit;
    [SerializeField] Button m_Restart;
    [SerializeField] Button m_Continue;
    [SerializeField] AnimationCurve m_Curve;

    void OnQuit()
    {
        m_QuitPopup.SetActive(true);
    }

    void ShutGameDown()
    {
        Application.Quit();
    }

    void OnQuitCancel()
    {
        m_QuitPopup.SetActive(false);
    }

    void OnRestart()
    {

    }

    void OnConitnue()
    {
        Close(null);
    }

    public override void Open(UnityAction done)
    {
        StartCoroutine(CoUtilize.VLerp((v) => pause.localScale = v, Vector3.zero, Vector3.one, 0.2f, () => Opend(done), m_Curve));
    }

    void Opend(UnityAction done)
    {
        done?.Invoke();
        Time.timeScale = 0;
    }

    public override void Close(UnityAction done)
    {
        Time.timeScale = 1;
        done?.Invoke();
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        m_QuitCancel.onClick.AddListener(OnQuitCancel);
        m_QuitOk.onClick.AddListener(ShutGameDown);
        m_Quit.onClick.AddListener(OnQuit);
        m_Restart.onClick.AddListener(OnRestart);
        m_Continue.onClick.AddListener(OnConitnue);
    }
}
