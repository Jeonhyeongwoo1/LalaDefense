using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;

public class TextHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Text target;
    public Color hover;
    public Color normal;
    public Color selected;

    public UnityEvent onClickEvent = new UnityEvent();
    public UnityEvent onEnterEvent = new UnityEvent();
    public UnityEvent onExitEvent = new UnityEvent();

//    public bool isSelected = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (target == null)
        {
            onEnterEvent?.Invoke();
            return;
        }

        target.color = hover;
        onEnterEvent?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (target == null)
        {
            onExitEvent?.Invoke();
            return;
        }

        target.color = normal;
        onExitEvent?.Invoke();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (target == null)
        {
            print("TEST");
            onClickEvent?.Invoke();
            return;
        }

        target.color = selected;
        onClickEvent?.Invoke();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (target == null)
        {
            target = GetComponent<Text>();
        }
    }

}
