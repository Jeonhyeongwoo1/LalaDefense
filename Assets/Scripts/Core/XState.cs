using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XState : States, IState
{
    public void Listen(string key, State call) { Watch(key, call); }
    public void Remove(string key, State call) { Stop(key, call); }
    public void Set(string key, object o) { SetState(key, o); }


    float m_AliveEnemyCount;
    public float aliveEnemyCount
    {
        get => m_AliveEnemyCount;
        set
        {
            m_AliveEnemyCount = value;
            if (m_AliveEnemyCount == 0)
            {
                Core.gameManager?.roundPlayer?.SetState(RoundPlayer.RoundState.Done);
            }
            Set(nameof(aliveEnemyCount), value);
        }
    }

    int m_TowerCount;
    public int towerCount
    {
        get => m_TowerCount;
        set { m_TowerCount = value; Set(nameof(towerCount), value); }
    }

    float m_Heart;
    public float heart
    {
        get => m_Heart;
        set
        {
            m_Heart = value;
            if (value == 0)
            {
                //Die
                Core.gameManager.GameOver();
            }
            Set(nameof(heart), value);
        }
    }

    float m_Money;
    public float money
    {
        get => m_Money;
        set { m_Money = value; Set(nameof(money), value); }
    }

    float m_Score;
    public float score
    {
        get => m_Score;
        set { m_Score = value; Set(nameof(score), value); }
    }

    int m_MissionCompleteCount;
    public int missionCompleteCount
    {
        get => m_MissionCompleteCount;
        set { m_MissionCompleteCount = value; Set(nameof(missionCompleteCount), value); }
    }

    float m_AudioVolume = 1;
    public float audioVolume
    {
        get => m_AudioVolume;
        set { m_AudioVolume = value; Set(nameof(audioVolume), value); }
    }

    bool m_Mute = false;
    public bool mute
    {
        get => m_Mute;
        set { m_Mute = value; Set(nameof(mute), value); }
    }

    string m_AudioName;
    public string audioName
    {
        get => m_AudioName;
        set { m_AudioName = value; Set(nameof(audioName), value); }
    }

    void SlientSet()
    {
        //        Listen(nameof(enemyCount), Notify);
    }

    // Start is called before the first frame update
    void Start()
    {
        SlientSet();
    }

    public void Notify(string key, object o)
    {
        Debug.Log("Key : " + key + "object : " + o);
    }


}
