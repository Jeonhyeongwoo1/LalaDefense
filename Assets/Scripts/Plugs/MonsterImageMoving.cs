using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterImageMoving : MonoBehaviour
{
    public Transform WavingTarget;
    public Transform MovingTarget;

    [SerializeField] float m_StartAxisX = 1308f;
    [SerializeField] float m_EndAxisX = -1300f;

    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float frequency = 20f;
    [SerializeField] float magnitude = 0.5f;
    [SerializeField] float m_WavingDuration = 20f;
    [SerializeField] float m_MovingDuration = 20f;

    Vector3 m_InitWavingTarget;
    Vector3 m_InitMovingTarget;

    // Start is called before the first frame update
    void Start()
    {
        m_InitWavingTarget = WavingTarget.localPosition;
        m_InitMovingTarget = MovingTarget.localPosition;
    }

    public void Close()
    {
        StopAllCoroutines();
        Init();
    }

    public void Init()
    {
        WavingTarget.localPosition = m_InitWavingTarget;
        MovingTarget.localPosition = m_InitMovingTarget;
        MovingTarget.localEulerAngles = Vector3.zero;
      //  Vector3 wPos = transform.TransformPoint(lPos);
    }

    public void StartMonsterImageAni()
    {
        Init();
        StartCoroutine(Waving(true));
        StartCoroutine(Moving());
    }

    IEnumerator Waving(bool isRight)
    {
        float elapsed = 0;
        Vector3 pos = WavingTarget.position;
        Vector3 init = WavingTarget.localPosition;
        float count = 0;
        while (count <= 3)
        {
            while (elapsed < m_WavingDuration)
            {
                elapsed += Time.deltaTime;
                pos += (isRight ? WavingTarget.right : -WavingTarget.right) * Time.deltaTime * moveSpeed;
                WavingTarget.position = pos + WavingTarget.up * Mathf.Sin(Time.time * frequency) * magnitude;
                yield return null;
            }

            WavingTarget.localPosition = init;
            pos = WavingTarget.position;
            elapsed = 0;
            count++;
        }
    }

    IEnumerator Moving()
    {
        float elapsed = 0f;
        Vector3 pos = MovingTarget.localPosition;
        Vector3 v = Vector3.zero;

        float dist = Mathf.Abs(m_EndAxisX - m_StartAxisX);
        float one_third = m_StartAxisX * 0.5f;
        float two_third = m_EndAxisX * 0.5f + 200f;
        float axisZ = 0;
        float angle = 30;
        float e1 = 0, e2 = 0;
        float count = 0;

        while (count <= 3)
        {
            while (elapsed < m_MovingDuration)
            {
                elapsed += Time.deltaTime;

                if (MovingTarget.localPosition.x < one_third && MovingTarget.localPosition.x > two_third)
                {
                    e1 += Time.deltaTime;
                    axisZ = Mathf.Lerp(0, -angle, e1 / 5);
                    MovingTarget.localEulerAngles = new Vector3(0, 0, axisZ);
                }

                if (MovingTarget.localPosition.x < two_third)
                {
                    e2 += Time.deltaTime;
                    axisZ = Mathf.Lerp(-angle, angle * 2, e2 / 5);
                    MovingTarget.localEulerAngles = new Vector3(0, 0, axisZ);
                }

                float axisX = Mathf.Lerp(m_StartAxisX, m_EndAxisX, elapsed / m_MovingDuration);
                MovingTarget.localPosition = new Vector3(axisX, pos.y, pos.z);
                yield return null;
            }

            e1 = 0; e2 = 0;
            elapsed = 0;
            MovingTarget.localPosition = new Vector3(m_StartAxisX, 0, 0);
            MovingTarget.localEulerAngles = Vector3.zero;
            count++;
        }
    }

    [ContextMenu("TEST")]
    public void Test()
    {
        StartCoroutine(Waving(true));
        StartCoroutine(Moving());
    }

}
