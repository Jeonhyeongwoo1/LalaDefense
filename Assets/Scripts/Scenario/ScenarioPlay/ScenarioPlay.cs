using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

public class ScenarioPlay : MonoBehaviour, IScenario
{
    public string scenarioName => typeof(ScenarioPlay).Name;

    public EnemyManager enemyManager;
    public TowerManager towerManager;
    public CinemachineVirtualCamera mainCam;

    [SerializeField] AudioListener m_AudioSource;
    //Test
    [SerializeField] int stageIndex = 1;


    bool IsLive(CinemachineVirtualCamera cam) => CinemachineCore.Instance.IsLive(cam) && !CinemachineCore.Instance.GetActiveBrain(0).IsBlending;

    public void ScenarioPrepare(UnityAction done)
    {
        QualitySettings.SetQualityLevel(0);
        Core.models.DefaultLoadModels();
        Core.plugs.DefaultLoadPlugs();
        LalaStarter.GetBlockSkybox()?.SetAlpha(0);
        enemyManager.SetMainCam(mainCam);
        done?.Invoke();
    }

    public void ScenarioStandbyCamera(UnityAction done)
    {
        StartCoroutine(CameraTransistion(mainCam, done));
    }

    public void ScenarioStart(UnityAction done)
    {
        Core.models.GetModel<HomeModel>()?.Close(null);
        Core.models.GetModel<Terrain>()?.Open(null);
        m_AudioSource.RandomChoice();

        Stage stage = Core.gameManager.stagePlayer.GetStage();
        if (stage == null)
        {
            stage = StagePlayer.Load(stageIndex);
            Core.gameManager.stagePlayer.SetStage(stage);
        }

        Core.gameManager.OnGameStart(enemyManager, towerManager, stage, null);
        done?.Invoke();
    }

    public void ScenarioStopCamera(UnityAction done)
    {
        done?.Invoke();
    }

    public void ScenarioStop(UnityAction done)
    {
        m_AudioSource.GetAudioSource().Stop();
        done?.Invoke();
    }

    // Start is called before the first frame update
    void Start()
    {
        Core.Ensure(() => Core.scenario.OnLoaded(this));
    }

    IEnumerator CameraTransistion(CinemachineVirtualCamera cam, UnityAction done)
    {
        cam.enabled = false;
        cam.enabled = true;
        while (!IsLive(cam)) { yield return null; }
        done?.Invoke();
    }

}
