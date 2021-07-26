using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMoving : MonoBehaviour
{
    [SerializeField] Transform m_Obj;
    [SerializeField] Vector3 m_Start;
    [SerializeField] Vector3 m_End;
    [SerializeField] bool isRepeat = true;

    Vector3 m_InitPos = Vector3.zero;

    IEnumerator RepeatMoving()
    {
        Vector3 dist = m_End - m_Start;
        float elapsed = 0;
        while (isRepeat)
        {
            elapsed += Time.deltaTime;
            m_Obj.localPosition = m_Start + new Vector3(dist.x != 0 ? Mathf.PingPong(elapsed, dist.x) : 0,
                                            dist.y != 0 ? Mathf.PingPong(elapsed, dist.y) : 0,
                                            dist.z != 0 ? Mathf.PingPong(elapsed, dist.z) : 0);
            yield return null;
        }
    }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        if (m_Obj == null)
        {
            m_Obj = gameObject.transform;
            m_InitPos = m_Obj.transform.localPosition;
        }
    }

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        StartCoroutine(RepeatMoving());
    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
        StopAllCoroutines();
        m_Obj.transform.localPosition = m_InitPos;
    }

}
