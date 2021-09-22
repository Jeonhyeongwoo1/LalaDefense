using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MissionPopup : BasePopup
{
    [SerializeField] Transform m_Popup;
    [SerializeField] Button m_Close;
    [SerializeField] GameObject m_MissionContentPrefab;
    [SerializeField] Transform m_MissionContent;
    [SerializeField] GameObject m_MissionNoDataText;
    [SerializeField] AnimationCurve m_Curve;
    float m_InitAxisY = -650;
    int stageNum = 0;

    public List<MissionContainer> missionList = new List<MissionContainer>();

    public void SetData()
    {
        Stage stage = Core.gameManager.stagePlayer.GetStage();
        if (stage.stageNum == stageNum)
        {
            SetMissionContainersData();
            return;
        }

        stageNum = stage.stageNum;
        Mission[] missions = stage.mission;

        DestoryMissionContents();

        if (missions == null)
        {
            m_MissionNoDataText.SetActive(true);
            return;
        }

        if (missions.Length == 1 && missions[0].missionName == "Default")
        {
            m_MissionNoDataText.SetActive(true);
            return;
        }

        foreach (Mission mission in missions)
        {
            if (mission.missionName == "Default") { continue; }
            GameObject go = Instantiate(m_MissionContentPrefab, m_MissionContentPrefab.transform.position, Quaternion.identity, m_MissionContent);
            MissionContainer missionContainer = go.GetComponent<MissionContainer>();
            missionContainer.Set(mission);
            missionList.Add(missionContainer);
        }
    }

    public void DestoryMissionContents()
    {
        if (m_MissionContent == null) { return; }

        foreach (Transform child in m_MissionContent)
        {
            GameObject.Destroy(child.gameObject);
        }

        if (missionList != null) { missionList.Clear(); }
    }

    public override void Open(UnityAction done)
    {
        SetData();
        gameObject.SetActive(true);
        StartCoroutine(CoUtilize.VLerp((v) => m_Popup.localPosition = v, m_Popup.transform.localPosition, Vector3.zero, 0.3f, done, m_Curve));

    }

    public override void Close(UnityAction done)
    {
        Core.plugs.GetPlugable<Popup>()?.RemoveOpenedPopup(this);
        StartCoroutine(CoUtilize.VLerp((v) => m_Popup.localPosition = v, m_Popup.transform.localPosition, new Vector3(0, m_InitAxisY, 0), 0.3f, () => Closed(done), m_Curve));
    }

    public int GetMissionCompleteCount()
    {
        int count = 0;
        foreach (MissionContainer v in missionList)
        {
            if (v.missionComplete) { count++; }
        }

        return count;
    }

    void Closed(UnityAction done)
    {
        done?.Invoke();
        gameObject.SetActive(false);
    }

    void OnDisable()
    {
        if (m_MissionNoDataText.activeSelf) { m_MissionNoDataText.gameObject.SetActive(false); }
        m_Popup.transform.localPosition = new Vector3(0, m_InitAxisY, 0);

        if (Core.state == null) { return; }
        Core.state.Remove(nameof(Core.state.towerCount), OnValueChanged);
        Core.state.Remove(nameof(Core.state.score), OnValueChanged);
        Core.state.Remove(nameof(Core.state.heart), OnValueChanged);
    }

    void OnValueChanged(string key, object o)
    {
        switch (key)
        {
            case nameof(Core.state.towerCount):
                SetMissionContainerValue("TowerCount", o.ToString());
                break;
            case nameof(Core.state.score):
                SetMissionContainerValue("Score", o.ToString());
                break;
            case nameof(Core.state.heart):
                SetMissionContainerValue("Heart", o.ToString());
                break;
        }
    }

    void SetMissionContainersData()
    {
        foreach (MissionContainer mission in missionList)
        {
            switch (mission.missionName)
            {
                case "TowerCount":
                    mission.SetContentValue(mission.missionName, Core.state.towerCount.ToString());
                    break;
                case "Score":
                    mission.SetContentValue(mission.missionName, Core.state.score.ToString());
                    break;
                case "Heart":
                    mission.SetContentValue(mission.missionName, Core.state.heart.ToString());
                    break;
            }
        }
    }

    void SetMissionContainerValue(string name, string value)
    {
        foreach (var v in missionList)
        {
            if (v == null) { continue; }
            if (v.missionName == name)
            {
                v.SetContentValue(name, value);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        m_Close.onClick.AddListener(() => Close(null));
        m_Close.gameObject.AddComponent<UXHover>();
    }

    void OnEnable()
    {
        Core.state.Listen(nameof(Core.state.towerCount), OnValueChanged);
        Core.state.Listen(nameof(Core.state.score), OnValueChanged);
        Core.state.Listen(nameof(Core.state.heart), OnValueChanged);
    }
}
