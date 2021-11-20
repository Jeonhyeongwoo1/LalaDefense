using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resolution : MonoBehaviour
{
    public enum Type { R1280x720, R1600x900, R1920x1080 }
    public Type curResoultion = Type.R1920x1080;

    public Toggle R1280x720;
    public Toggle R1600x900;
    public Toggle R1920x1080;
    public Toggle FullScreen;

    public TextHandler R1280x720Txt;
    public TextHandler R1600x900Txt;
    public TextHandler R1920x1080Txt;
    public TextHandler FullScreenTxt;

    void SetFullScreen(bool isOn)
    {
        Debug.Log("Change Full Screen " + isOn);

        Core.state.fullScreen = isOn;
        FullScreen.isOn = isOn;
        SetResolution(curResoultion, isOn);
    }

    public void Init()
    {
        Core.state.fullScreen = Screen.fullScreen ? true : false;
        FullScreen.isOn = Screen.fullScreen ? true : false;

        R1280x720.onValueChanged.AddListener((v) => Set(Type.R1280x720, R1280x720, v));
        R1600x900.onValueChanged.AddListener((v) => Set(Type.R1600x900, R1600x900, v));
        R1920x1080.onValueChanged.AddListener((v) => Set(Type.R1920x1080, R1920x1080, v));
        FullScreen.onValueChanged.AddListener((v) => SetFullScreen(v));

        R1280x720Txt.onClickEvent.AddListener(() => R1280x720.isOn = true);
        R1600x900Txt.onClickEvent.AddListener(() => R1600x900.isOn = true);
        R1920x1080Txt.onClickEvent.AddListener(() => R1920x1080.isOn = true);
        FullScreenTxt.onClickEvent.AddListener(() => FullScreen.isOn = !Core.state.fullScreen);
    }

    public void Set(Type type, Toggle toggle, bool isOn)
    {
        if (!isOn) { return; }
        if (curResoultion == type) { return; }

        Debug.Log("Change Qulity Level : " + type);

        toggle.isOn = true;
        SetResolution(type, Core.state.fullScreen);
        curResoultion = type;

        //PlayerPrefs...

    }

    void SetResolution(Type type, bool isFullScreen)
    {

        switch (type)
        {
            case Type.R1280x720:
                Screen.SetResolution(1280, 720, isFullScreen);
                break;
            case Type.R1600x900:
                Screen.SetResolution(1600, 900, isFullScreen);
                break;
            case Type.R1920x1080:
                Screen.SetResolution(1920, 1080, isFullScreen);
                break;
        }
    }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        Init();
    }
}
