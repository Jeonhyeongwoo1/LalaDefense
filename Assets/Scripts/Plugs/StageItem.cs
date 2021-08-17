using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class StageItem : MonoBehaviour
{
    public enum StageType { None, Completed, Open, Lock }

    public StageType stageType;
    public int itemIndex = 0;

    [SerializeField] Transform m_Completed;
    [SerializeField] Transform m_Lock;
    [SerializeField] Transform m_Open;
    [SerializeField, Range(0, 1)] float m_ShakeDuration;
    [SerializeField] float m_ShakeRange;

    public void InitStageItem(StageType stageType, float starCount = 0)
    {
        switch (stageType)
        {
            case StageType.Completed:
                m_Completed.gameObject.SetActive(true);
                Transform stars = m_Completed.GetChild(1);
                for (int i = 0; i < starCount; i++)
                {
                    stars.GetChild(i).gameObject.SetActive(true);
                }
                break;
            case StageType.Open:
                m_Open.gameObject.SetActive(true);
                break;
            case StageType.Lock:
                m_Lock.gameObject.SetActive(true);
                break;
        }
    }

    IEnumerator Shaking()
    {
        float elapsed = 0;
        Vector3 value = Vector3.zero;
        Vector3 init = transform.localPosition;

        while (elapsed < m_ShakeDuration)
        {
            elapsed += Time.deltaTime;
            value = new Vector3(Random.Range(-m_ShakeRange, m_ShakeRange), Random.Range(-m_ShakeRange, m_ShakeRange));
            transform.localPosition += value;
            yield return null;
        }

        transform.localPosition = init;
    }

    void OnClickItem()
    {
        if (stageType == StageType.Lock)
        {
            StartCoroutine(Shaking());
            return;
        }

        SendMessageUpwards("OnClickStageItem", itemIndex);

    }

    void Start()
    {
        m_Completed.GetComponent<Button>().onClick.AddListener(OnClickItem);
        m_Open.GetComponent<Button>().onClick.AddListener(OnClickItem);
        m_Lock.GetComponent<Button>().onClick.AddListener(OnClickItem);
    }

}
