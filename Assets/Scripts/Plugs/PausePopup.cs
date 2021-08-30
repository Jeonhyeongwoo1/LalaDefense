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

    //게임종료
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
        Close(() => Core.gameManager.GameRestart());
    }

    void OnConitnue()
    {
        Close(null);
    }

    public override void Open(UnityAction done)
    {
        StartCoroutine(CoUtilize.VLerp((v) => pause.localScale = v, Vector3.zero, Vector3.one, 0.2f, () => Opened(done), m_Curve));
    }

    void Opened(UnityAction done)
    {
        done?.Invoke();
        Core.gameManager.GamePause(true);
    }

    public override void Close(UnityAction done)
    {
        Core.plugs.GetPlugable<Popup>().RemoveOpenedPopup(this);
        Core.gameManager.GamePause(false);
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
