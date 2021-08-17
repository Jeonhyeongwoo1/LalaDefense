using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class StagePopup : BasePopup
{
    public StageNavigation stageNavigation;
    public StageReady stageReadyPopup;
    public List<StageList> stageList = new List<StageList>();

    [SerializeField] MonsterImageMoving monsterImage;
    [SerializeField] Transform m_Background;
    [SerializeField] Button m_Left;
    [SerializeField] Button m_Right;
    [SerializeField] Button m_Close;
    [SerializeField] Button m_Home;
    [SerializeField] AnimationCurve m_Curve;

    [SerializeField, Range(0, 1)] float m_SlideDuration = 0.3f;
    [SerializeField] float m_MoveDistance = 1829;

    float m_CurrentStage = 0;

    public override void Open(UnityAction done)
    {
        StartCoroutine(CoUtilize.VLerp((v) => m_Background.localScale = v, Vector3.zero, Vector3.one, 0.2f, () => Opend(done), m_Curve));
    }

    void Opend(UnityAction done)
    {
        monsterImage.StartMonsterImageAni();
        done?.Invoke();
    }

    public override void Close(UnityAction done)
    {
        monsterImage.Close();
        gameObject.SetActive(false);
    }

    public void OnClickStageItem(int itemIndex)
    {
        Debug.Log("Stage Popup Open. Item Index : " + itemIndex);

        Stage stage = StagePlayer.Load(itemIndex);
        StagePlayer.Instance.stage = stage;
        stageReadyPopup.SetStageInfo(itemIndex.ToString(), stage.userHeart);
        stageReadyPopup.Open(null);
    }

    void MoveStage(bool isLeft)
    {
        if (m_CurrentStage == 0 && isLeft == true) { return; }
        if (stageList.Count - 1 == m_CurrentStage && isLeft == false) { return; }

        m_Left.enabled = false;
        m_Right.enabled = false;

        StartCoroutine(Moving(isLeft, ChangedStageList));
        stageNavigation.ChangePage(m_CurrentStage);
    }

    void ChangedStageList()
    {
        if (m_CurrentStage != 0)
        {
            m_Left.enabled = true;
        }
        else
        {
            m_Left.enabled = false;
        }

        if (m_CurrentStage == stageList.Count - 1)
        {
            m_Right.enabled = false;
        }
        else
        {
            m_Right.enabled = true;
        }

    }

    IEnumerator Moving(bool isLeft, UnityAction done)
    {
        m_CurrentStage = isLeft ? --m_CurrentStage : ++m_CurrentStage;

        float elapsed = 0f;
        Vector3 v = Vector3.zero;
        List<float> destination = new List<float>();

        for (var i = 0; i < stageList.Count; i++)
        {
            destination.Add(stageList[i].transform.localPosition.x);
        }

        while (elapsed < m_SlideDuration)
        {
            elapsed += Time.deltaTime;

            for (var i = 0; i < stageList.Count; i++)
            {
                Transform tr = stageList[i].transform;
                v = new Vector3(Mathf.Lerp(tr.localPosition.x, isLeft ? destination[i] + m_MoveDistance : destination[i] + (-m_MoveDistance), m_Curve.Evaluate(elapsed / m_SlideDuration)), -40f, 0);
                tr.localPosition = v;
            }

            yield return null;
        }

        done?.Invoke();
    }

    void SetStageListPosition()
    {
        for (int i = 0; i < stageList.Count; i++)
        {
            stageList[i].transform.localPosition = new Vector3(i * m_MoveDistance, -40f, 0);
        }
    }

    void OnEnable()
    {
        m_CurrentStage = 0;
        m_Left.enabled = false;
        m_Right.enabled = true;
        SetStageListPosition();
        stageNavigation.ChangePage(0);
    }

    void OnValidate()
    {
        SetStageListPosition();
    }

    void GoHome() { }

    void Start()
    {
        m_Left.onClick.AddListener(() => MoveStage(true));
        m_Right.onClick.AddListener(() => MoveStage(false));
        m_Close.onClick.AddListener(() => Close(null));
        m_Home.onClick.AddListener(() => GoHome());
    }

}
