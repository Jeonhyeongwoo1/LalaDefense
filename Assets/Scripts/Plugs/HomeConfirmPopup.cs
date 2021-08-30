using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HomeConfirmPopup : BasePopup
{
    public StagePopup stagePopup;
    [SerializeField] Button m_Cancel;
    [SerializeField] Button m_Ok;
    [SerializeField] Text m_Content;

    public override void Open(UnityAction done)
    {
        gameObject.SetActive(true);
        done?.Invoke();
    }

    public override void Close(UnityAction done)
    {
        done?.Invoke();
        gameObject.SetActive(false);
    }

    public void SetContent(string content) => m_Content.text = content;

    public void GoHome()
    {
        stagePopup.Close(null);
        Close(() => Core.gameManager.GoHome());
    }

    // Start is called before the first frame update
    void Start()
    {
        m_Cancel.onClick.AddListener(() => Close(null));
        m_Ok.onClick.AddListener(GoHome);

    }

}
