using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;

public class Menu : BaseTheme
{

    [Serializable]
    public class Menus
    {
        public string menuName;
        public Transform menu;
    }

    [SerializeField] Menus[] m_Menus;
    [SerializeField, Range(0, 1)] float m_OpeningMenu;
    [SerializeField] AnimationCurve m_Curve;

    public override void Open(UnityAction done)
    {
        StartCoroutine(OpeningMenus(done));
    }

    public override void Close(UnityAction done)
    {
        Core.plugs.GetPlugable<Theme>()?.RemoveOpenedTheme(this);
        done?.Invoke();
        ScaleInit();
        gameObject.SetActive(false);
    }

    public Transform GetMenuTranform(string menuName)
    {
        foreach (Menus tr in m_Menus)
        {
            if (menuName == tr.menuName)
            {
                return tr.menu;
            }
        }

        return null;
    }

    IEnumerator OpeningMenus(UnityAction done)
    {
        foreach (var m in m_Menus)
        {
            yield return CoUtilize.VLerp((v) => m.menu.localScale = v, Vector3.zero, Vector3.one, 0.5f, null, m_Curve);
        }

        done?.Invoke();
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        MenuSetup();
    }

    void ScaleInit()
    {
        foreach (var v in m_Menus) { v.menu.localScale = Vector3.zero; }
    }

    void MenuSetup()
    {
        Popup popup = Core.plugs.GetPlugable<Popup>();

        foreach (var v in m_Menus)
        {
            switch (v.menuName)
            {
                case "SettingsPopup":
                    v.menu.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        if (popup.IsOpenedPopup<SettingsPopup>()) { return; }
                        popup.Open<SettingsPopup>();
                    });
                    break;
                case "StagePopup":
                    v.menu.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        if (popup.IsOpenedPopup<StagePopup>()) { return; }
                        popup.Open<StagePopup>();
                    });
                    break;
                case "MissionPopup":
                    v.menu.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        if (popup.IsOpenedPopup<MissionPopup>()) { return; }
                        popup.Open<MissionPopup>();
                    });
                    break;
            }
        }
    }
}
