using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class RoundPlayer : Singleton<RoundPlayer>
{
    public enum RoundState { None, Ready, Play, Done }

    EnemyInfo curEnemyInfo;
    EnemyManager enemyManager;

    [SerializeField] RoundState roundState = RoundState.None;
    [SerializeField] List<Round> rounds = new List<Round>();
    [SerializeField] float roundWaitTime = 2;

    public void SetState(RoundState roundState) => this.roundState = roundState;
    public RoundState GetState() => roundState;
    public EnemyInfo GetCurEnemyInfo() => curEnemyInfo;

    void Log(string content)
    {
        Debug.Log(content);
    }

    public void ReadyRound(EnemyManager enemyManager, List<Round> roundInfo)
    {
        Log("Ready Round");
        roundState = RoundState.Ready;
        rounds = roundInfo;
        this.enemyManager = enemyManager;
    }

    public void PlayRound(UnityAction done)
    {
        Log("Play Round");
        roundState = RoundState.Play;
        StartCoroutine(PlayingRound(done));
    }

    void RoundDone(UnityAction done)
    {
        Log("Round Done");
        roundState = RoundState.Done;
        done?.Invoke();
    }

    IEnumerator PlayingRound(UnityAction done)
    {
        EnemyInfo enemyInfo = new EnemyInfo();

        for (int i = 0; i < rounds.Count; i++)
        {
            roundState = RoundState.Play;
            yield return new WaitForSeconds(roundWaitTime);
            Log("Play Round Index : " + rounds[i].index);

            enemyInfo.enemyType = (EnemyType)Enum.Parse(typeof(EnemyType), rounds[i].monsterName);
            enemyInfo.health = rounds[i].monsterHealth;
            enemyInfo.rewardMoney = rounds[i].monsterMoney;
            enemyInfo.speed = rounds[i].monsterSpeed;
            enemyManager.EnemySpawn(enemyInfo, rounds[i].monsterCount);

            while (roundState != RoundState.Done) { yield return null; }
        }

        RoundDone(done);
    }

}
