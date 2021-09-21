using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionContent : MonoBehaviour
{
    public GameObject contentPrefab;
    public Transform missionContent;
    [SerializeField, Range(0, 1)] float m_TextShowSpeed;

    public void Set(Mission[] missions)
    {
        StartCoroutine(ShowMissionContent(missions));
    }

    public void DestoryMissionContents()
    {
        if (missionContent == null) { return; }

        foreach (Transform child in missionContent)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
        DestoryMissionContents();
    }

    IEnumerator ShowMissionContent(Mission[] missions)
    {
        if (missions == null) { yield break; }

        for (int i = 0; i < missions.Length; i++)
        {
            GameObject g = Instantiate(contentPrefab, contentPrefab.transform.position, Quaternion.identity, missionContent.transform);
            Text text = g.GetComponent<Text>();
            text.text += (i + 1) + ". ";

            for (int j = 0; j < missions[i].missionContent.Length; j++)
            {
                char c = missions[i].missionContent[j];
                text.text += c.ToString();
                yield return new WaitForSeconds(m_TextShowSpeed);
            }
        }
    }

}
