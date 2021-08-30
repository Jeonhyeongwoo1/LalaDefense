using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resolution : MonoBehaviour
{
    public enum Type { R1280x1024, R1600x1200, R1920x1080 }
    public Type curResoultion = Type.R1920x1080;

    public Toggle R1280x1024;
    public Toggle R1600x1200;
    public Toggle R1920x1080;

    public TextHandler R1280x1024Txt;
    public TextHandler R1600x1200Txt;
    public TextHandler R1920x1080Txt;

    //추후에 추가..
    public bool fullscreen;

    public void Init()
    {
        R1280x1024.onValueChanged.AddListener((v) => Set(Type.R1280x1024, R1280x1024, v));
        R1600x1200.onValueChanged.AddListener((v) => Set(Type.R1600x1200, R1600x1200, v));
        R1920x1080.onValueChanged.AddListener((v) => Set(Type.R1920x1080, R1920x1080, v));
        R1280x1024Txt.onClickEvent.AddListener(() => R1280x1024.isOn = true);
        R1600x1200Txt.onClickEvent.AddListener(() => R1600x1200.isOn = true);
        R1920x1080Txt.onClickEvent.AddListener(() => R1920x1080.isOn = true);
    }

    public void Set(Type type, Toggle toggle, bool isOn)
    {
        if (!isOn) { return; }
        if (curResoultion == type) { return; }

        Debug.Log("Change Qulity Level : " + type);

        toggle.isOn = true;

        switch (type)
        {
            case Type.R1280x1024:
                Screen.SetResolution(1280, 1024, true);
                break;
            case Type.R1600x1200:
                Screen.SetResolution(1600, 1200, true);
                break;
            case Type.R1920x1080:
                Screen.SetResolution(1920, 1080, true);
                break;
        }

        curResoultion = type;

        //PlayerPrefs...

    }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        Init();
    }
}
