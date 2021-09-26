using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class StageReady : BasePopup
{
    public StagePopup stagePopup;
    public GameObject screenDim;
    public Transform popup;
    public UnityAction closeEvent = null;

    [SerializeField] MissionContent missionContent;
    [SerializeField] RollingNumber rollingNumber;
    [SerializeField] Button m_Close;
    [SerializeField] Button m_Start;

    [SerializeField] Text m_StageTitle;
    [SerializeField] Text m_MissionContent;

    [SerializeField] float m_PopupInitAxisY = -1035;
    [SerializeField, Range(0, 1)] float m_OpenDuration = 0.3f;

    [SerializeField] AnimationCurve m_Curve;

    Stage m_Stage;

    public void SetStageInfo(Stage stage)
    {
        m_Stage = stage;
        m_StageTitle.text = "STAGE " + stage.stageNum;
    }

    public override void Open(UnityAction done)
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }

        screenDim.SetActive(true);
        popup.gameObject.SetActive(true);

        popup.localPosition = new Vector3(0, -1035, 0);
        popup.GetComponent<CanvasGroup>().alpha = 0;

        StartCoroutine(CoUtilize.Lerp((v) => popup.localPosition = new Vector3(0, v, 0),
                        popup.localPosition.y, 0, m_OpenDuration, null, m_Curve));
        StartCoroutine(CoUtilize.Lerp((v) => popup.GetComponent<CanvasGroup>().alpha = v,
                        0, 1, m_OpenDuration, () => Opened(done), m_Curve));
    }

    void Opened(UnityAction done)
    {
        missionContent.Set(m_Stage.mission);
        rollingNumber.StartRolling(m_Stage.userHeart);

        done?.Invoke();
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
        missionContent.DestoryMissionContents();
        missionContent.StopAllCoroutines();
        screenDim.SetActive(false);
    }

    //수정해야함.
    void OnClickStart()
    {

        if (Core.scenario.GetCurScenario().scenarioName != nameof(ScenarioPlay))
        {
            Core.gameManager.stagePlayer.SetStage(m_Stage);
            Core.scenario.OnLoadSceneAsync(nameof(ScenarioPlay));
        }
        else
        {
            Core.gameManager.StartSelectedGame(m_Stage);
        }
        closeEvent?.Invoke();
        stagePopup.Close(null);
        closeEvent = null;
        gameObject.SetActive(false);
    }



    // Start is called before the first frame update
    void Start()
    {
        m_Close.onClick.AddListener(() => Close(Closed));
        m_Start.onClick.AddListener(OnClickStart);
    }

}
