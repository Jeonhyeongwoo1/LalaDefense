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

    public string missionCondition;
    public string missionName;
    public bool missionComplete = false;


    public void Set(Mission mission)
    {
        m_Content.text = mission.missionContent;
        missionName = mission.missionName;
        missionCondition = mission.missionCondition;
        switch (mission.missionName)
        {
            case "TowerCount":
                SetContentValue(mission.missionName, Core.state.towerCount.ToString());
                break;
            case "Heart":
                SetContentValue(mission.missionName, Core.state.heart.ToString());
                break;
            case "Score":
                SetContentValue(mission.missionName, Core.state.score.ToString());
                break;
        }
    }

    public void SetContentValue(string name, string value)
    {
        switch (name)
        {
            case "TowerCount":
                m_ContentValue.text = "(" + value + " / " + missionCondition + ")";
                break;
            case "Heart":
                m_ContentValue.text = "(" + value + " / " + Core.gameManager.stagePlayer.GetStage().userHeart + ")";
                break;
            case "Score":
                m_ContentValue.text = "(" + value + " / " + missionCondition + ")";
                break;
        }

        try
        {
            float v = int.Parse(value);
            float condition = int.Parse(missionCondition);

            if (v >= condition)
            {
                m_CheckBox.SetActive(true);
                m_ContentValue.color = m_MissionCler;
                missionComplete = true;
            }
            else
            {
                m_CheckBox.SetActive(false);
                m_ContentValue.color = Color.white;
                missionComplete = false;
            }
        }
        catch (Exception e)
        {
            Debug.Log("Error" + e);
            m_CheckBox.SetActive(false);
            m_ContentValue.color = Color.white;
        }
    }

}
