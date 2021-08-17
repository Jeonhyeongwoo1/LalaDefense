using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

[Serializable]
public class Round
{
    public int index;
    public string monsterName;
    public string type;
    public int monsterHealth;
    public int monsterMoney;
    public int monsterSpeed;
    public int monsterCount;
}

[Serializable]
public class Stage
{
    public int index;
    public int userMoney;
    public int userHeart;
    public Round[] roundInfo;
}

public class StagePlayer : Singleton<StagePlayer>
{
    public enum StageState { None, Ready, Play, Done }

    public Stage stage;
    public RoundPlayer roundPlayer;

    [SerializeField] StageState m_StageState = StageState.None;
    UnityAction m_DoneEvent;

    public void SetState(StageState stage) => m_StageState = stage;
    public StageState GetState() => m_StageState;

    void Log(string content)
    {
        Debug.Log(content);
    }

    public void RestartStage()
    {
        PlayStage();
    }

    public void ReadyStage(EnemyManager enemyManager, UnityAction done)
    {
        Log("Ready Stage");
        m_StageState = StageState.Ready;
        m_DoneEvent = done;

        Theme theme = FindObjectOfType<Theme>();
        theme.towerStore.gameObject.SetActive(true);
        UserInfoUI userInfo = theme.userInfoUI;
        userInfo.gameObject.SetActive(true);
        userInfo.SetUserInfo(stage.userHeart, stage.userMoney);

        List<Round> round = new List<Round>();
        round.AddRange(stage.roundInfo);
        roundPlayer.ReadyRound(enemyManager, round);

        PlayStage();
    }

    void PlayStage()
    {
        Log("Play Stage");
        m_StageState = StageState.Play;
        roundPlayer.PlayRound(StageDone);
    }

    void StageDone()
    {
        Log("Stage Done Index : " + stage.index);
        m_StageState = StageState.Done;
        m_DoneEvent?.Invoke();
    }

    public static Stage Load(int stageNumber)
    {
        string path = "Json/Stage/S" + stageNumber;
        TextAsset data = Resources.Load<TextAsset>(path);

        if (data == null)
        {
            Debug.Log("StageNumber : " + stageNumber + " Stage Not Found Path : " + path);
            return null;
        }

        Stage stage = JsonUtility.FromJson<Stage>(data.text);

        if (stage == null)
        {
            Debug.Log("StageNumber : " + stageNumber + " Stage Not Found");
            return null;
        }

        return stage;
    }

    void Start()
    {
        roundPlayer = RoundPlayer.Instance;
    }

}
