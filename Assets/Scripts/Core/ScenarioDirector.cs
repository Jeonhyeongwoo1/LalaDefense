using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public interface IScenario
{
    string scenarioName { get; }
    void ScenarioPrepare(UnityAction done);
    void ScenarioStandbyCamera(UnityAction done);
    void ScenarioStart(UnityAction done);
    void ScenarioStopCamera(UnityAction done);
    void ScenarioStop(UnityAction done);
}

public class ScnearioTemplate : MonoBehaviour, IScenario
{
    public string scenarioName => typeof(ScnearioTemplate).Name;

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
    }
    
    public void ScenarioStopCamera(UnityAction done)
    {
        done?.Invoke();
    }
    
    public void ScenarioStop(UnityAction done)
    {
        done?.Invoke();
    }
}

public class ScenarioDirector : Singleton<ScenarioDirector>
{
    IScenario m_AcitveScenario;
    IScenario m_ClosingScenario;

    public void Log(string content)
    {
        Debug.Log(content);
    }
    
    public void OnLoaded(IScenario scenario)
    {
        if (m_AcitveScenario == null)
        {
            m_AcitveScenario = scenario;
            scenario.ScenarioPrepare(() => StandbyCamera(scenario));
            return;
        }

        m_ClosingScenario = m_AcitveScenario;
        m_AcitveScenario = null;
        OnLoaded(scenario);
        UnLoad(m_ClosingScenario);
    }

    void StandbyCamera(IScenario scenario)
    {
        if (scenario != null)
        {
            Log("Standby Camera : " + scenario.scenarioName);
            scenario.ScenarioStandbyCamera(() => ScneraioStart(scenario));
        }
    }

    void ScneraioStart(IScenario scenario)
    {
        if (scenario != null)
        {
            Log("Scneraio Start : " + scenario.scenarioName);
            scenario.ScenarioStart(() => ReadyScenario(scenario));
        }
    }

    void ReadyScenario(IScenario scenario)
    {
        Log("Scenario Ready Complete : " + scenario.scenarioName);
    }

    void UnLoad(IScenario scenario)
    {
        if(scenario != null)
        {
            Log("Scenario Pending Unload : " + scenario.scenarioName);
            scenario.ScenarioStopCamera(() => StopCamera(scenario));
        }
    }

    void StopCamera(IScenario scenario)
    {
        if (scenario != null)
        {
            Log("Scenario Stop Camera : " + scenario.scenarioName);
            scenario.ScenarioStop(() => StopScenario(scenario));
        }
    }

    void StopScenario(IScenario scenario)
    {
        if (scenario != null)
        {
            Log("Scenario Stop :" + scenario.scenarioName);
            m_AcitveScenario = null;
            UnloadSceneAsync(scenario.scenarioName);
            return;
        }
    }

    public void UnloadSceneAsync(string scenarioName)
    {
        SceneManager.UnloadSceneAsync(scenarioName);
    }

    public void OnLoadSceneAsync(string scenarioName)
    {
        if(m_AcitveScenario != null)
        {
            UnLoad(m_AcitveScenario);
        }

        SceneManager.LoadSceneAsync(scenarioName, LoadSceneMode.Additive);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
