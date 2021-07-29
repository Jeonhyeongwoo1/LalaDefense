using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terrain : MonoBehaviour, IModel
{
    public Node nodes;

    public void SelectNode(Transform node)
    {
        Transform n = nodes.GetNode(node);
        n.gameObject.SetActive(false);
        nodes.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
