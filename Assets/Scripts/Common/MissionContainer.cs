using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class MissionContainer : MonoBehaviour
{
    [SerializeField] GameObject m_CheckBox;
    [SerializeField] Text m_Content;
    [SerializeField] Text m_ContentValue;
    [SerializeField] Color m_MissionCler;

    string m_MissionCondition;
    string m_MissionName;

    public void Init(Mission mission)
    {
        m_Content.text = mission.missionContent;
        m_MissionCondition = mission.missionCondition;
        m_MissionName = mission.missionName;

        float value = 0;
        switch (mission.missionName)
        {
            case "TowerCount":
                value = Core.state.towerCount;
                break;
            case "Heart":
                value = Core.state.heart;
                break;
            case "Score":
                value = Core.state.score;
                break;
        }

        SetData(mission.missionName, value);
    }

    public void SetData(string missionName, float value)
    {
        if (missionName != m_MissionName) { return; }
        
        try
        {
            float condition = int.Parse(m_MissionCondition);
            m_ContentValue.text = "(" + value + " / " + condition + ")";

            if (value >= condition)
            {
                m_CheckBox.SetActive(true);
                m_ContentValue.color = m_MissionCler;
            }
            else
            {
                m_CheckBox.SetActive(false);
                m_ContentValue.color = Color.white;
            }
        }
        catch (Exception e)
        {
            Debug.Log("Error" + e);
            m_CheckBox.SetActive(false);
            m_ContentValue.color = Color.white;
        }
    }

    void OnValueChanged(string key, object o)
    {
        float value = 0;
        try
        {
            value = float.Parse(o.ToString());
        }
        catch (Exception e)
        {
            Debug.Log("Error : " + e);
            value = 0;
        }

        switch (key)
        {
            case nameof(Core.state.towerCount):
                SetData("TowerCount", value);
                break;
            case nameof(Core.state.score):
                SetData("Score", value);
                break;
            case nameof(Core.state.heart):
                SetData("Heart", value);
                break;
        }
    }


    void OnEnable()
    {
        Core.state.Listen(nameof(Core.state.towerCount), OnValueChanged);
        Core.state.Listen(nameof(Core.state.score), OnValueChanged);
        Core.state.Listen(nameof(Core.state.heart), OnValueChanged);
    }

    void OnDisable()
    {
        if (Core.state == null) { return; }
        Core.state.Remove(nameof(Core.state.towerCount), OnValueChanged);
        Core.state.Remove(nameof(Core.state.score), OnValueChanged);
        Core.state.Remove(nameof(Core.state.heart), OnValueChanged);
    }

}
