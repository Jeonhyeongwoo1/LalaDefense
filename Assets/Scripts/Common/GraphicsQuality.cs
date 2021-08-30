using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphicsQuality : MonoBehaviour
{
    public enum Type { Low, Medium, High }
    public Type curQuality = Type.Medium;

    public Toggle low;
    public Toggle medium;
    public Toggle high;

    public TextHandler lowTxt;
    public TextHandler mediumTxt;
    public TextHandler highTxt;

    public void Init()
    {
        low.onValueChanged.AddListener((v) => Set(Type.Low, low, v));
        medium.onValueChanged.AddListener((v) => Set(Type.Medium, medium, v));
        high.onValueChanged.AddListener((v) => Set(Type.High, high, v));
        lowTxt.onClickEvent.AddListener(() => low.isOn = true);
        mediumTxt.onClickEvent.AddListener(() => medium.isOn = true);
        highTxt.onClickEvent.AddListener(() => high.isOn = true);
    }

    public void Set(Type type, Toggle toggle, bool isOn)
    {
        if (!isOn) { return; }
        if (curQuality == type) { return; }

        Debug.Log("Change Qulity Level : " + type);

        toggle.isOn = true;

        switch (type)
        {
            case Type.Low:
                QualitySettings.SetQualityLevel(0);
                break;
            case Type.Medium:
                QualitySettings.SetQualityLevel(3);
                break;
            case Type.High:
                QualitySettings.SetQualityLevel(5);
                break;
        }

        curQuality = type;

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
