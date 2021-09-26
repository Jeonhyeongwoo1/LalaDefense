using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;

public class AudioListener : MonoBehaviour
{
    [SerializeField] AudioSource m_AudioSource;

    public AudioClip[] audioSources;

    public AudioSource RandomChoice()
    {
        int value = UnityEngine.Random.Range(0, audioSources.Length);
        m_AudioSource.clip = audioSources[value];
        Core.state.audioName = m_AudioSource.clip.name;
        return m_AudioSource;
    }

    public AudioSource GetAudioSource() => m_AudioSource;

    public void ChangeAudioClip(AudioClip audioClip)
    {
        m_AudioSource.clip = audioClip;
    }

    void OnEnable()
    {
        Core.state.Listen(nameof(Core.state.audioVolume), OnValueChanged);
        Core.state.Listen(nameof(Core.state.mute), OnValueChanged);
        Core.state.Listen(nameof(Core.state.audioName), OnValueChanged);
    }

    void OnDisable()
    {
        if (Core.state == null) { return; }
        Core.state.Remove(nameof(Core.state.audioVolume), OnValueChanged);
        Core.state.Remove(nameof(Core.state.mute), OnValueChanged);
        Core.state.Remove(nameof(Core.state.audioName), OnValueChanged);
    }

    void OnValueChanged(string key, object o)
    {
        switch (key)
        {
            case nameof(Core.state.audioName):
                string name = o.ToString();
                for (int i = 0; i < audioSources.Length; i++)
                {
                    if (audioSources[i].name == name)
                    {
                        m_AudioSource.Pause();
                        m_AudioSource.clip = audioSources[i];

                        if (!Core.state.mute)
                        {
                            m_AudioSource.PlayDelayed(3);
                        }

                    }
                }

                break;
            case nameof(Core.state.audioVolume):
                try
                {
                    float volume = float.Parse(o.ToString());
                    m_AudioSource.volume = volume;
                }
                catch (Exception e)
                {
                    Debug.Log("Audio Error : " + e);
                    m_AudioSource.volume = 0;
                }
                break;
            case nameof(Core.state.mute):
                try
                {
                    bool muteOn = bool.Parse(o.ToString());
                    if (muteOn)
                    {
                        m_AudioSource.Pause();
                    }
                    else
                    {
                        m_AudioSource.Play();
                    }
                }
                catch (Exception e)
                {
                    Debug.Log("Audio Error : " + e);
                    m_AudioSource.Stop();
                }
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (m_AudioSource == null)
        {
            m_AudioSource = GetComponent<AudioSource>();
        }
    }
}
