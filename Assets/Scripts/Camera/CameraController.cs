using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public Transform activeCamera;

    [SerializeField, Range(0, 20)] float m_TranslateMultiplier;
    [SerializeField, Range(0, 20)] float m_ScrollMultiplier;

    [Header("[Direction Max, Min Value]")]
    [SerializeField] float m_MaxX = 42f;
    [SerializeField] float m_MinX = 16.9f;
    [SerializeField] float m_MaxZ = 37.3f;
    [SerializeField] float m_MinZ = 2.3f;

    [Header("[Zoom]")]
    [SerializeField] float m_ZoomOut = 28;
    [SerializeField] float m_ZoomIn = 15f;

    /// <summary>
    /// LateUpdate is called every frame, if the Behaviour is enabled.
    /// It is called after all Update functions have been called.
    /// </summary>
    void LateUpdate()
    {
        Vector3 v = activeCamera.transform.position;

        if (v.x > m_MaxX) { activeCamera.transform.position = new Vector3(m_MaxX, v.y, v.z); return; }
        if (v.x < m_MinX) { activeCamera.transform.position = new Vector3(m_MinX, v.y, v.z); return; }
        if (v.z > m_MaxZ) { activeCamera.transform.position = new Vector3(v.x, v.y, m_MaxZ); return; }
        if (v.z < m_MinZ) { activeCamera.transform.position = new Vector3(v.x, v.y, m_MinZ); return; }

        if (Input.GetKey(KeyCode.W))
        {
            activeCamera.Translate(Vector3.forward * Time.deltaTime * m_TranslateMultiplier, Space.World);
        }

        if (Input.GetKey(KeyCode.S))
        {
            activeCamera.Translate(Vector3.back * Time.deltaTime * m_TranslateMultiplier, Space.World);
        }

        if (Input.GetKey(KeyCode.A))
        {
            activeCamera.Translate(Vector3.left * Time.deltaTime * m_TranslateMultiplier, Space.World);
        }

        if (Input.GetKey(KeyCode.D))
        {
            activeCamera.Translate(Vector3.right * Time.deltaTime * m_TranslateMultiplier, Space.World);
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            Vector3 pos = transform.position;
            pos.y -= scroll * m_ScrollMultiplier * Time.deltaTime * 10f;
            pos.y = Mathf.Clamp(pos.y, m_ZoomIn, m_ZoomOut);
            transform.position = pos;
        }
    }

}
