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
    float m_InitAxisY = 650;

    Mission[] m_Missions;

    public void SetData()
    {
        Stage stage = Core.gameManager.stagePlayer.GetStage();
        Mission[] missions = stage.mission;

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
            Text content = go.transform.GetChild(1).GetComponent<Text>();
            content.text = mission.missionContent;
        }
    }

    public void DestoryMissionContents()
    {
        if (m_MissionContent == null) { return; }

        foreach (Transform child in m_MissionContent)
        {
            GameObject.Destroy(child.gameObject);
        }
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

    void Closed(UnityAction done)
    {
        done?.Invoke();
        gameObject.SetActive(false);
    }

    void OnDisable()
    {
        DestoryMissionContents();
        if (m_MissionNoDataText.activeSelf) { m_MissionNoDataText.gameObject.SetActive(false); }
        m_Popup.transform.localPosition = new Vector3(0, m_InitAxisY, 0);
    }

    // Start is called before the first frame update
    void Start()
    {
        m_Close.onClick.AddListener(() => Close(null));
        m_Close.gameObject.AddComponent<UXHover>();
    }

}
