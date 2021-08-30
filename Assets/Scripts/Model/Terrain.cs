using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Terrain : MonoBehaviour, IModel
{
    public string modelName => typeof(Terrain).Name;
    public Node nodes;
    public WayPoint wayPoint;

    public void SelectNode(Transform node)
    {
        Transform n = nodes.GetNode(node);
        n.gameObject.SetActive(false);
        nodes.gameObject.SetActive(false);
    }

    public void Open(UnityAction done)
    {
        gameObject.SetActive(true);
        done?.Invoke();
    }

    public void Close(UnityAction done)
    {
        done?.Invoke();
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        Core.Ensure(() => Core.models.SetModel(this));
    }
}
