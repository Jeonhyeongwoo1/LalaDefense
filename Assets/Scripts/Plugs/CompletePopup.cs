using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class CompletePopup : BasePopup
{
    public Transform popup;

    [SerializeField] TextMeshProUGUI m_Score;
    [SerializeField] TextMeshProUGUI m_Heart;
    [SerializeField] Button m_Close;
    [SerializeField] Button m_Home;
    [SerializeField] Button m_Restart;
    [SerializeField] Button m_Next;
    [SerializeField] AnimationCurve m_Curve;

    public override void Open(UnityAction done)
    {
        Theme theme = Core.plugs.GetPlugable<Theme>();
        UserInfoUI userInfo = theme.GetTheme<UserInfoUI>();
        m_Score.text = userInfo.score == 0 ? "0" : string.Format("{0:#,###}", userInfo.score);
        m_Heart.text = "x" + userInfo.heart.ToString();

        

        StartCoroutine(CoUtilize.VLerp((v) => popup.localScale = v, Vector3.zero, Vector3.one, 0.2f, done, m_Curve));
    }

    public override void Close(UnityAction done)
    {
        Popup p = Core.plugs.GetPlugable<Popup>();
        p.RemoveOpenedPopup(this);
        done?.Invoke();
        gameObject.SetActive(false);
    }

    //마지막일 경우 안보이는??
    void OnNext()
    {
        Close(() => Core.gameManager.OnNextGame());
    }

    void GoHome()
    {
        Close(() => Core.gameManager.GoHome());
    }

    void OnRestart()
    {
        Close(() => Core.gameManager.GameRestart());
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
