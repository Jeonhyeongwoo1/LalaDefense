using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class StageReady : BasePopup
{
    public GameObject screenDim;
    public Transform popup;
    
    [SerializeField] RollingNumber rollingNumber;
    [SerializeField] Button m_Close;
    [SerializeField] Button m_Start;

    [SerializeField] Text m_StageTitle;
    [SerializeField] Text m_MissionContent;

    [SerializeField] float m_PopupInitAxisY = -1035;
    [SerializeField, Range(0, 1)] float m_OpenDuration = 0.3f;

    [SerializeField] AnimationCurve m_Curve;

    int m_Life = 5;

    public void SetStageInfo(string stage, int life, string missionContent = null)
    {
        m_Life = life;
        m_StageTitle.text = "STAGE " + stage;
        m_MissionContent.text = missionContent == null ? m_MissionContent.text : missionContent;
    }

    public override void Open(UnityAction done)
    {
        screenDim.SetActive(true);
        popup.gameObject.SetActive(true);

        popup.localPosition = new Vector3(0, -1035, 0);
        popup.GetComponent<CanvasGroup>().alpha = 0;

        StartCoroutine(CoUtilize.Lerp((v) => popup.localPosition = new Vector3(0, v, 0),
                        popup.localPosition.y, 0, m_OpenDuration, null, m_Curve));
        StartCoroutine(CoUtilize.Lerp((v) => popup.GetComponent<CanvasGroup>().alpha = v,
                        0, 1, m_OpenDuration, () => rollingNumber.StartRolling(m_Life), m_Curve));

    }

    public override void Close(UnityAction done)
    {
        rollingNumber.StopRolling();
        StartCoroutine(CoUtilize.Lerp((v) => popup.localPosition = new Vector3(0, v, 0),
                        popup.localPosition.y, m_PopupInitAxisY, m_OpenDuration, null, m_Curve));
        StartCoroutine(CoUtilize.Lerp((v) => popup.GetComponent<CanvasGroup>().alpha = v,
                        1, 0, m_OpenDuration, done, m_Curve));
    }

    void Closed()
    {
        screenDim.SetActive(false);
    }

    void OnClickStart()
    {
        ScenarioDirector.Instance.OnLoadSceneAsync(nameof(ScenarioPlay));
        gameObject.SetActive(false);
        transform.parent.parent.gameObject.SetActive(false); //StagePopup
    }

    // Start is called before the first frame update
    void Start()
    {
        m_Close.onClick.AddListener(() => Close(Closed));
        m_Start.onClick.AddListener(OnClickStart);
    }

}
