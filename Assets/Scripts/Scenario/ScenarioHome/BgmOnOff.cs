using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BgmOnOff : MonoBehaviour
{
    public bool isOn
    {
        get => m_isOn;
        set
        {
            m_isOn = value;
            SetBgmOnOff(value);
        }
    }

    [SerializeField] bool m_isOn = true;
    [SerializeField] AudioSource m_AudioSource;
    [SerializeField] Button m_Bgm;
    [SerializeField] Sprite m_On;
    [SerializeField] Sprite m_Off;

    public void ChangeImage(bool on)
    {
        m_Bgm.GetComponent<Image>().sprite = on ? m_On : m_Off;
    }

    void SetBgmOnOff(bool on)
    {

        m_Bgm.GetComponent<Image>().sprite = on ? m_On : m_Off;

        if (on)
        {
            m_AudioSource.PlayDelayed(2);
        }
        else
        {
            m_AudioSource.Stop();
        }
    }

    void Start()
    {
        m_Bgm.onClick.AddListener(() => isOn = !isOn);
    }

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        if (m_AudioSource != null)
        {
            m_AudioSource.volume = Core.state.audioVolume;
        }
    }

}
