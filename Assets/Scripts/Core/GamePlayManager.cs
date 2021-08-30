using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
    public StagePlayer stagePlayer;
    public RoundPlayer roundPlayer => stagePlayer?.roundPlayer;

    public void Log(string content)
    {
        Debug.Log(content);
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

        stagePlayer.OnNextStageReady(stage);
    }

    public void GameOver()
    {
        Log("Game over");

        stagePlayer.GameOverStage();

        Terrain terrain = Core.models.GetModel<Terrain>();
        terrain.nodes.ActiveAllNodes(true);

        Popup popup = Core.plugs.GetPlugable<Popup>();
        popup.Open<LosePopup>();
    }


    public void GamePause(bool isPause)
    {
        Time.timeScale = isPause ? 0 : 1;
    }

    public void GameRestart()
    {
        Log("Game Restart");

        Terrain terrain = Core.models.GetModel<Terrain>();
        terrain.nodes.ActiveAllNodes(true);

        Theme theme = Core.plugs.GetPlugable<Theme>();
        Stage s = stagePlayer.GetStage();
        theme.GetTheme<UserInfoUI>()?.SetUserInfo(s.userHeart, s.userMoney, 0);

        stagePlayer.OnRestartStage();
    }

    public void GameComplete()
    {
        stagePlayer.GameOverStage();

        Terrain terrain = Core.models.GetModel<Terrain>();
        terrain.nodes.ActiveAllNodes(true);
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

    [ContextMenu("Gameover")]
    public void Test1()
    {
        GameOver();
    }

    [ContextMenu("Gohome")]
    public void Test2()
    {
        GoHome();
    }

    [ContextMenu("gamerestart")]
    public void Test3()
    {
        GameRestart();
    }

}
