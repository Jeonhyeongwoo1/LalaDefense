using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class Bgm : MonoBehaviour
{
    public bool mute
    {
        get => m_MuteOn;
        set { Mute(value); }
    }

    public UnityAction bgmTextOnclickEvent;
    bool m_MuteOn = false;

    [Header("[GameObject Component]")]
    [SerializeField] Slider m_Volume;
    [SerializeField] Image m_VolumeFill;
    [SerializeField] Image m_VolumeHandle;
    [SerializeField] Button m_MuteBtn;
    [SerializeField] Image m_MuteBgmImage;
    [SerializeField] TextMeshProUGUI m_Off;
    [SerializeField] TextMeshProUGUI m_On;
    [SerializeField] TMP_Dropdown m_Dropdown;

    [Header("Resources")]
    [SerializeField] Sprite m_VolumeHandleOn;
    [SerializeField] Sprite m_VolumeHandleOff;
    [SerializeField] Sprite m_VolumeFillOn;
    [SerializeField] Sprite m_VolumeFillOff;
    [SerializeField] Sprite m_DisableSound;
    [SerializeField] Sprite m_EnableSound;
    [SerializeField] Sprite m_SwitchOff;
    [SerializeField] Sprite m_SwitchOn;

    [Space]
    [SerializeField] float m_OnX;
    [SerializeField] float m_OffX;
    [SerializeField, Range(0, 1)] float m_MuteDuration;
    //AudioListener audioListener = Camera.main.GetComponent<AudioListener>();

    string m_CurrentAudio;

    public void Init()
    {
        Vector3 v = m_MuteBtn.transform.localPosition;
        m_MuteBtn.transform.localPosition = new Vector3(m_MuteOn ? m_OffX : m_OnX, v.y, v.z);
        m_MuteOn = !m_MuteOn;
        SetBgm();
        m_Volume.onValueChanged.AddListener((v) => Set(v));
        m_Dropdown.onValueChanged.AddListener(OnDropDownValueChanged);
        m_MuteBtn.onClick.AddListener(() => Mute(m_MuteOn));
       
    }

    void SetBgm()
    {
        for (int i = 0; i < m_Dropdown.options.Count; i++)
        {
            if (m_Dropdown.options[i].text == Core.state.audioName)
            {
                m_Dropdown.value = i;
            }
        }
    }

    void OnDropDownValueChanged(int value)
    {
        Core.state.audioName = m_Dropdown.options[value].text;
    }

    public void Mute(bool on) // Mute
    {
        Vector3 pos = m_MuteBtn.transform.localPosition;

        m_MuteBtn.image.sprite = on ? m_SwitchOff : m_SwitchOn;
        m_MuteBgmImage.sprite = on ? m_DisableSound : m_EnableSound;

        m_VolumeFill.sprite = on ? m_VolumeFillOff : m_VolumeFillOn;
        m_VolumeHandle.sprite = on ? m_VolumeHandleOff : m_VolumeHandleOn;
        m_Volume.enabled = !on;

        m_Off.gameObject.SetActive(on);
        m_On.gameObject.SetActive(!on);

        StartCoroutine(CoUtilize.Lerp((v) =>
                        m_MuteBtn.transform.localPosition = new Vector3(v, pos.y, pos.z),
                        pos.x,
                        on ? m_OffX : m_OnX,
                        m_MuteDuration));

        m_MuteOn = !on;
        Core.state.mute = on;
    }

    public void Set(float amount)
    {
        Core.state.audioVolume = amount;
    }
    
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        Init();
    }
}
