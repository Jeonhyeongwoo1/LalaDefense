using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class RoundPlayer : MonoBehaviour
{
    public int roundCurIndex
    {
        get => m_RoundCurIndex;
        set => m_RoundCurIndex = value;
    }

    public int roundCurScore
    {
        get
        {
            if (roundCurIndex == 0)
            {
                return m_Rounds[roundCurIndex].score;
            }

            return m_Rounds[roundCurIndex - 1].score;
        }
    }

    public int monsterCurRewardMoney
    {
        get
        {
            if (roundCurIndex == 0)
            {
                return (int)m_Rounds[roundCurIndex].enemyRewardMoney;
            }

            return (int)m_Rounds[roundCurIndex - 1].enemyRewardMoney;
        }
    }

    public EnemyManager enemyManager;
    public enum RoundState { None, Ready, Play, Done }

    [SerializeField] RoundState m_RoundState = RoundState.None;
    [SerializeField] List<Round> m_Rounds = new List<Round>();
    [SerializeField] float m_RoundWaitTime = 2;
    [SerializeField] int m_RoundCurIndex = 0;

    EnemyInfo m_CurEnemyInfo;
    
    public void SetState(RoundState roundState) => this.m_RoundState = roundState;
    public RoundState GetState() => m_RoundState;
    public EnemyInfo GetCurEnemyInfo() => m_CurEnemyInfo;

    void Log(string content)
    {
        Debug.Log(content);
    }

    public void GameOverRound()
    {
        StopAllCoroutines();
        m_RoundState = RoundState.Done;
    }

    public void RestartReadyRound()
    {
        StopAllCoroutines();
        m_RoundState = RoundState.Ready;
    }

    public void ReadyRound(List<Round> roundInfo)
    {
        Log("Ready Round");
        m_RoundState = RoundState.Ready;
        m_Rounds = roundInfo;
    }

    public void PlayRound(UnityAction done)
    {
        Log("Play Round");
        m_RoundState = RoundState.Play;
        StartCoroutine(PlayingRound(done));
    }

    void RoundDone(UnityAction done)
    {
        Log("Round Done");
        m_RoundState = RoundState.Done;
        done?.Invoke();
    }

    IEnumerator PlayingRound(UnityAction done)
    {
        yield return new WaitForSeconds(m_RoundWaitTime);

        Theme theme = Core.plugs.GetPlugable<Theme>();
        if(!theme.IsOpenedTheme<RoundInfoUI>()) 
        {
            theme.Open<RoundInfoUI>();
        }

        if(!theme.IsOpenedTheme<SpeedUI>())
        {
            theme.Open<SpeedUI>();
        }
       
        EnemyInfo enemyInfo = new EnemyInfo();
        RoundInfoUI roundInfoUI = theme.GetTheme<RoundInfoUI>();

        for (int i = 0; i < m_Rounds.Count; i++)
        {
            m_RoundState = RoundState.Play;
            roundInfoUI.SetRoundInfo(i + 1, m_Rounds.Count, m_Rounds[i].enemyCount, m_Rounds[i].enemyCount);
            yield return OpeningRoundUI(i + 1);
            yield return new WaitForSeconds(m_RoundWaitTime);
            Log("Playing Round Index : " + m_Rounds[i].index);

            roundCurIndex = m_Rounds[i].index;
            enemyInfo.enemyType = (EnemyType)Enum.Parse(typeof(EnemyType), m_Rounds[i].enemyType);
            enemyInfo.level = (EnemyInfo.Level)Enum.Parse(typeof(EnemyInfo.Level), m_Rounds[i].enemyLevel);
            enemyInfo.health = m_Rounds[i].enemyHealth;
            enemyInfo.rewardMoney = m_Rounds[i].enemyRewardMoney;
            enemyInfo.speed = m_Rounds[i].enemySpeed;
            enemyManager.EnemySpawn(enemyInfo, m_Rounds[i].enemyCount);

            while (m_RoundState != RoundState.Done) { yield return null; }
        }

        RoundDone(done);
    }

    IEnumerator OpeningRoundUI(int curRound)
    {
        Theme theme = Core.plugs.GetPlugable<Theme>();

        if (theme.IsOpenedTheme<RoundUI>()) { yield break; }

        bool isDone = false;
        RoundUI roundUI = theme.GetTheme<RoundUI>();
        theme.Open<RoundUI>(() => roundUI.Close(() => isDone = true));
        roundUI.SetRoundInfo(curRound, m_Rounds.Count);
        while (!isDone) { yield return null; }
    }

}
