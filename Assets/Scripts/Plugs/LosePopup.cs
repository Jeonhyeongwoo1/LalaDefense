using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class LosePopup : BasePopup
{
    public Transform popup;

    [SerializeField] TextMeshProUGUI m_Score;
    [SerializeField] Button m_Close;
    [SerializeField] Button m_Home;
    [SerializeField] Button m_Restart;
    [SerializeField] AnimationCurve m_Curve;

    public override void Open(UnityAction done)
    {
        Theme theme = Core.plugs.GetPlugable<Theme>();
        float score = theme.GetTheme<UserInfoUI>().score;
        m_Score.text = score == 0 ? "0" : string.Format("{0:#,###}", score);

        gameObject.SetActive(true);
        StartCoroutine(CoUtilize.VLerp((v) => popup.localScale = v, Vector3.zero, Vector3.one, 0.2f, done, m_Curve));
    }

    public override void Close(UnityAction done)
    {
        Core.plugs.GetPlugable<Popup>()?.RemoveOpenedPopup(this);
        done?.Invoke();
        gameObject.SetActive(false);
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
        m_Home.onClick.AddListener(GoHome);
        m_Restart.onClick.AddListener(OnRestart);
    }

}
