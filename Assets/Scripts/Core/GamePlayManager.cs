using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class GamePlayManager : MonoBehaviour
{
    public bool bossAppearAniPlaying
    {
        get;
        set;
    }

    public StagePlayer stagePlayer;
    public RoundPlayer roundPlayer => stagePlayer?.roundPlayer;
    [Range(0, 2)] public float gameSpeed = 1; //0 ~ 3
    EnemyManager m_EnemyManager = null;
    TowerManager m_TowerManager = null;

    public void Log(string content)
    {
        Debug.Log(content);
    }

    public void OnGameStart(EnemyManager enemyManager, TowerManager towerManager, Stage stage, UnityAction done)
    {
        if (stage == null)
        {
            Log("Not Select Stage : ");

            return;
        }

        m_EnemyManager = enemyManager;
        m_TowerManager = towerManager;

        Theme theme = Core.plugs.GetPlugable<Theme>();
        theme.Open<TowerStore>();
        UserInfoUI userInfo = theme.GetTheme<UserInfoUI>();
        Core.state.heart = stage.userHeart;
        Core.state.money = stage.userMoney;
        Core.state.score = 0;
        theme.Open<UserInfoUI>();
        theme.Open<Menu>();

        stagePlayer.OnReadyStage(enemyManager, towerManager, stage, done);

    }

    public void StartSelectedGame(Stage stage)
    {
        Log("Start Seleced Game" + stage.stageNum);

        ResetGame(stage.userHeart, stage.userMoney);
        stagePlayer.OnNextStageReady(stage);
    }

    void ResetGame(float userHeart, float money)
    {
        Theme theme = Core.plugs.GetPlugable<Theme>();
        Core.state.heart = userHeart;
        Core.state.money = money;
        Core.state.score = 0;

        TowerStore towerStore = theme.GetTheme<TowerStore>();
        if (!towerStore.isOpenCircleBtn) { towerStore.OnCloseTowerStore(); }
        Core.plugs.GetPlugable<Popup>()?.CloseOpenedPopups();

        Terrain terrain = Core.models.GetModel<Terrain>();
        terrain.nodes.ActiveAllNodes(true);

        DestoryTowerAndEnemy();
    }

    public void OnNextGame()
    {
        if (stagePlayer.GetState() != StagePlayer.StageState.Done) { return; }

        int curNumber = stagePlayer.GetStage().stageNum;
        Stage stage = StagePlayer.Load(curNumber + 1);

        //Max 일때 처리
        //Max Or Error
        if (stage == null)
        {
            Debug.Log("Not Found Stage");
            return;
        }

        Log("Next Stage Play");

        ResetGame(stage.userHeart, stage.userMoney);
        stagePlayer.OnNextStageReady(stage);
    }

    public void GameOver()
    {
        Log("Game over");

        DestoryTowerAndEnemy();
        stagePlayer.GameOverStage();

        Terrain terrain = Core.models.GetModel<Terrain>();
        terrain.nodes.ActiveAllNodes(true);

        Theme theme = Core.plugs.GetPlugable<Theme>();
        TowerStore towerStore = theme.GetTheme<TowerStore>();
        towerStore.OnCloseTowerStore();

        Popup popup = Core.plugs.GetPlugable<Popup>();
        popup.CloseOpenedPopups(() => popup.Open<LosePopup>());
    }

    public void GamePause(bool isPause)
    {
        Time.timeScale = isPause ? 0 : gameSpeed;
    }

    public void GameRestart()
    {
        Log("Game Restart");

        Terrain terrain = Core.models.GetModel<Terrain>();
        terrain.nodes.ActiveAllNodes(true);

        Stage s = stagePlayer.GetStage();
        Core.state.heart = s.userHeart;
        Core.state.money = s.userMoney;
        Core.state.score = 0;

        Theme theme = Core.plugs.GetPlugable<Theme>();
        TowerStore towerStore = theme.GetTheme<TowerStore>();
        towerStore.OnCloseTowerStore();
        DestoryTowerAndEnemy();

        stagePlayer.OnRestartStage();
    }

    public void DestoryTowerAndEnemy()
    {
        m_EnemyManager.DestroyEnemy();
        m_TowerManager.DestroyImmediateAllTower();
    }

    public void GameComplete()
    {
        DestoryTowerAndEnemy();
        stagePlayer.GameOverStage();
        Terrain terrain = Core.models.GetModel<Terrain>();
        terrain.nodes.ActiveAllNodes(true);

        if (Core.gameManager.gameSpeed != 1)
        {
            Core.gameManager.gameSpeed = 1; //Default
            Time.timeScale = 1;
        }
    }

    public void GoHome()
    {
        Log("GoHome");

        GameComplete();

        Core.plugs.GetPlugable<Theme>()?.CloseOpenedThemes();
        Core.plugs.GetPlugable<Popup>()?.CloseOpenedPopups();

        Core.models.GetModel<HomeModel>()?.Open(null);
        Core.models.GetModel<Terrain>()?.Close(null);
        Core.scenario.OnLoadSceneAsync(nameof(ScenarioHome));
    }

    public int GetMissionCompleteCount()
    {
        Stage stage = stagePlayer.GetStage();
        Mission[] missions = stage.mission;
        int count = 0, condition = 0;

        foreach (Mission mission in missions)
        {
            if (mission.missionName == "Default") { continue; }

            try
            {
                condition = int.Parse(mission.missionCondition);
            }
            catch (Exception e)
            {
                Debug.Log("Error : " + e);
                condition = 0;
                return 0;
            }

            switch (mission.missionName)
            {
                case "TowerCount":
                    if (Core.state.towerCount >= condition) { count++; }
                    break;
                case "Heart":
                    if (Core.state.heart >= condition) { count++; }
                    break;
                case "Score":
                    if (Core.state.score >= condition) { count++; }
                    break;
            }

        }

        return count;
    }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        if (stagePlayer == null)
        {
            GameObject go = new GameObject("StagePlayer");
            go.transform.SetParent(transform);
            go.AddComponent<StagePlayer>();
            stagePlayer = go.GetComponent<StagePlayer>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            if (Core.scenario.GetCurScenario()?.scenarioName != nameof(ScenarioPlay)) { return; }

            Popup popup = Core.plugs.GetPlugable<Popup>();
            if (!popup.IsOpenedPopup<PausePopup>())
            {
                popup.Open<PausePopup>();
            }
        }
    }


}
