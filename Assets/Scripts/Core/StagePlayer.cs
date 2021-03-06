using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

[Serializable]
public class Round
{
    public int index;
    public int score;
    public int enemyCount;
    public string enemyLevel;
    public string enemyType;
    public float enemyHealth;
    public float enemyRewardMoney;
    public float enemySpeed;
}

[Serializable]
public class Mission
{
    public string missionName;
    public string missionCondition;
    public string missionContent;
}

[Serializable]
public class Stage
{
    public int stageNum;
    public int userMoney;
    public int userHeart;
    public Mission[] mission;
    public Round[] roundInfo;
}

public class StagePlayer : MonoBehaviour
{
    public int missionCount
    {
        get => m_Stage.mission.Length;
    }

    public enum StageState { None, Ready, Play, Done }
    public RoundPlayer roundPlayer;

    [SerializeField] Stage m_Stage;
    [SerializeField] StageState m_StageState = StageState.None;

    UnityAction m_DoneEvent = null;

    void Log(string content)
    {
        Debug.Log(content);
    }

    public void SetStage(Stage stage) => m_Stage = stage;
    public Stage GetStage() => m_Stage;

    public void SetState(StageState stage) => m_StageState = stage;
    public StageState GetState() => m_StageState;

    public void GameOverStage()
    {
        m_StageState = StageState.Done;
        roundPlayer.GameOverRound();
    }

    public void OnRestartStage()
    {
        roundPlayer.RestartReadyRound();
        OnPlayStage();
    }

    public void OnNextStageReady(Stage stage)
    {
        Log("Next Stage Ready");

        m_Stage = stage;  
        List<Round> round = new List<Round>();
        round.AddRange(m_Stage.roundInfo);
        roundPlayer.GameOverRound();
        roundPlayer.ReadyRound(round);

        OnPlayStage();
    }

    public void OnReadyStage(EnemyManager enemyManager, TowerManager towerManager, Stage stage, UnityAction done)
    {
        Log("Ready Stage");
        m_Stage = stage;
        m_StageState = StageState.Ready;
        m_DoneEvent = done;
        List<Round> round = new List<Round>();
        round.AddRange(m_Stage.roundInfo);
        roundPlayer.ReadyRound(round);
        roundPlayer.enemyManager = enemyManager;

        OnPlayStage();
    }

    void OnPlayStage()
    {
        Log("Play Stage Num : " + m_Stage.stageNum);
        m_StageState = StageState.Play;
        roundPlayer.PlayRound(OnStageDone);
    }

    void OnStageDone()
    {
        Log("Stage Done. Stage Num : " + m_Stage.stageNum);
        m_StageState = StageState.Done;
        OnStageCompleted();
    }

    void OnStageCompleted()
    {
        Popup popup = Core.plugs.GetPlugable<Popup>();
        popup.CloseOpenedPopups(() => popup.Open<CompletePopup>());
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

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        if (roundPlayer == null)
        {
            GameObject go = new GameObject("RoundPlayer");
            go.transform.SetParent(transform);
            go.AddComponent<RoundPlayer>();
            roundPlayer = go.GetComponent<RoundPlayer>();
        }
    }

}
