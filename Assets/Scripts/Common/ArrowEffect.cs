using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ArrowEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public UXHover uXHover;
    [SerializeField] float m_AxisY;
    [SerializeField] float m_Speed;

    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        CancelInvoke();
        uXHover.OnHover = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InvokeRepeating("Effect", 5f, 10f);
    }

    void Start()
    {
        if (uXHover == null)
        {
            uXHover = GetComponent<UXHover>();
        }

        InvokeRepeating("Effect", 5f, 10f);
    }

    void Effect()
    {
        if (gameObject.activeSelf)
        {
            StartCoroutine(Moving());
        }
    }


    IEnumerator Moving()
    {
        float elapsed = 0f;
        while (elapsed < 2f)
        {
            elapsed += Time.deltaTime;
            transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Repeat(elapsed * m_Speed, m_AxisY), 0);
            yield return null;
        }
    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
        StopAllCoroutines();
        CancelInvoke();
    }

}
