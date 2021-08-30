using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SettingsPopup : BasePopup
{
    public GraphicsQuality quality;
    public Resolution resolution;
    public Bgm bgm;

    [SerializeField] Transform m_Popup;
    [SerializeField] Button m_Close;

    public override void Open(UnityAction done)
    {
        if (!m_Popup.gameObject.activeSelf)
        {
            m_Popup.gameObject.SetActive(true);
        }

        done?.Invoke();
    }

    public override void Close(UnityAction done)
    {
        Core.plugs.GetPlugable<Popup>()?.RemoveOpenedPopup(this);
        done?.Invoke();
        gameObject.SetActive(false);
    }

    //초기 셋팅을 여기서 한다.
    void SettingsInit()
    {
        //PlayerPrefs
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        m_Close.onClick.AddListener(() => Close(null));

        SettingsInit();
    }
}
