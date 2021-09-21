using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Tower))]
public class ObjectPool : MonoBehaviour
{
    [SerializeField] GameObject m_Poolingobj;

    Queue<Transform> m_PoolingObjQueue = new Queue<Transform>();
    List<Transform> m_PoolingObjList = new List<Transform>();

    public int GetCount() => m_PoolingObjQueue.Count;

    public void DestroyAll()
    {
        if (m_PoolingObjQueue == null) { return; }
        if (m_PoolingObjQueue.Count == 0) { return; }

        foreach (var v in m_PoolingObjQueue)
        {
            DestroyImmediate(v.gameObject);
        }

        m_PoolingObjQueue.Clear();
    }

    public void Initialize(Transform parent, int count)
    {
        for (int i = 0; i < count; i++)
        {
            m_PoolingObjQueue.Enqueue(Create(parent).transform);
        }
    }

    public Transform Get(Transform parent, Vector3 startPosition)
    {
        if (m_PoolingObjQueue.Count <= 0)
        {
            m_PoolingObjQueue.Enqueue(Create(parent).transform);
        }

        Transform tr = m_PoolingObjQueue.Dequeue();
        if (tr == null) { return null; }

        tr.SetParent(parent);
        tr.position = startPosition;
        tr.gameObject.SetActive(true);
        return tr;
    }

    public List<Transform> GetAllCreatedList()
    {
        if (m_PoolingObjList.Count == 0) { return null; }

        return m_PoolingObjList;
    }

    public void Return(Transform poolingObj)
    {
        if (poolingObj.gameObject.activeSelf) { poolingObj.gameObject.SetActive(false); }
        m_PoolingObjQueue.Enqueue(poolingObj);
    }

    GameObject Create(Transform parent)
    {
        GameObject go = Instantiate(m_Poolingobj, transform.position, Quaternion.identity, parent);
        go.SetActive(false);
        m_PoolingObjList.Add(go.transform);
        return go;
    }
}
