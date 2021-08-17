using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class RollingNumber : MonoBehaviour
{
    [SerializeField, Range(0, 3)] float m_RollingDuration = 2f;
    [SerializeField] AnimationCurve m_Curve;

    public void StartRolling(float lifeNumber)
    {
        transform.GetComponent<Text>().text = "X 99";
        StartCoroutine(Rolling(lifeNumber));
    }

    public void StopRolling()
    {
        StopAllCoroutines();
        transform.GetComponent<Text>().text = "X 5";
    }

    IEnumerator Rolling(float lifeNumber)
    {
        float maxNumber = 99;
        float elapsed = 0;
        int cur = 0;

        while (elapsed < m_RollingDuration)
        {
            elapsed += Time.deltaTime;
            cur = (int)Mathf.SmoothStep(maxNumber, lifeNumber, m_Curve.Evaluate(elapsed / m_RollingDuration));
            transform.GetComponent<Text>().text = "X " + cur.ToString();
            yield return null;
        }

    }

    [ContextMenu("TEST")]
    public void Test()
    {
        StartCoroutine(Rolling(5));
    }

}
