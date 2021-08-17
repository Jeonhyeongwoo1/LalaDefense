using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScenarioPlay : MonoBehaviour, IScenario
{
    public string scenarioName => typeof(ScenarioPlay).Name;

    public EnemyManager enemyManager;
    public TowerManager towerManager;

    public void ScenarioPrepare(UnityAction done)
    {
        done?.Invoke();
    }

    public void ScenarioStandbyCamera(UnityAction done)
    {
        done?.Invoke();
    }

    public void ScenarioStart(UnityAction done)
    {
        done?.Invoke();
        StagePlayer.Instance.ReadyStage(enemyManager, OnStageCompleted);
    }

    public void ScenarioStopCamera(UnityAction done)
    {
        done?.Invoke();
    }

    public void ScenarioStop(UnityAction done)
    {
        done?.Invoke();
    }

    public void OnStageCompleted()
    {
        Popup popup = FindObjectOfType<Popup>();
        popup.completePopup.gameObject.SetActive(true);
        popup.completePopup.Open(null);
    }

    // Start is called before the first frame update
    void Start()
    {
        ScenarioDirector.Instance.OnLoaded(this);
//        Core.Instance.scenario.OnLoaded(this);
       // ModelDirector.Instance.DefaultModel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
