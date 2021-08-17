using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    public List<Transform> wayPoints = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        Transform[] child = GetComponentsInChildren<Transform>();
        wayPoints.AddRange(child);
        wayPoints.RemoveAt(0);
    }
}
