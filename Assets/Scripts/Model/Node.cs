using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Node : MonoBehaviour
{
    public bool isActiveAllNode;
    public Material magicCircle;
    public List<Transform> nodes = new List<Transform>();
    [SerializeField] Vector3 m_PositionOffset;

    Transform m_SelectedNode;

    public Vector3 GetBuildPosition()
    {
        return m_SelectedNode != null ? m_SelectedNode.position + m_PositionOffset : m_PositionOffset;
    }

    public Transform GetNode(string name)
    {
        m_SelectedNode = nodes.Find((v) => v.name == name);
        return m_SelectedNode;
    }

    public Transform GetNode(Transform node)
    {
        m_SelectedNode = nodes.Find((v) => v == node);
        return m_SelectedNode;
    }

    public void ActiveAllNodes(bool active)
    {
        nodes.ForEach((v) => v.gameObject.SetActive(active));
        isActiveAllNode = active;
    }

    // Start is called before the first frame update
    void Start()
    {
        Transform[] childs = GetComponentsInChildren<Transform>();
        nodes.AddRange(childs);
        nodes.RemoveAt(0);
        nodes.ForEach((v) =>
        {
            Material[] materials = v.GetComponent<MeshRenderer>().materials;
            materials[0] = magicCircle;
            v.GetComponent<MeshRenderer>().materials = materials;
            v.gameObject.SetActive(true);
        });
    }

}
