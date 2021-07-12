using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public interface IScenario
{
    void ScenarioPrepare(UnityAction done);
    void ScenarioStandbyCamera(UnityAction done);
    void ScenarioStart(UnityAction done);
    void ScenarioStopCamera(UnityAction done);
    void ScenarioStop(UnityAction done);
}

public class ScnearioTemplate : MonoBehaviour, IScenario
{
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
    [SerializeField] string m_ScneraioName;

    IScenario m_AcitveScenario;
    IScenario m_PendingScenario;

    public void Log(string content)
    {
        Debug.Log(content);
    }

    public void OnLoaded(IScenario scenario, string name)
    {
        if (m_AcitveScenario == null)
        {
            m_AcitveScenario = scenario;
            m_ScneraioName = name;
            scenario.ScenarioPrepare(() => StandbyCamera(scenario));
            return;
        }

        m_ScneraioName = name;
        m_PendingScenario = m_AcitveScenario;
        m_AcitveScenario = null;
        OnLoaded(scenario, name);
        UnLoad(m_PendingScenario);
    }

    void StandbyCamera(IScenario scenario)
    {
        if (scenario != null)
        {
            Log("Standby Camera : " + m_ScneraioName);
            scenario.ScenarioStandbyCamera(() => ScneraioStart(scenario));
        }
    }

    void ScneraioStart(IScenario scenario)
    {
        if (scenario != null)
        {
            Log("Scneraio Start : " + m_ScneraioName);
            scenario.ScenarioStart(() => ReadyScenario(scenario));
        }
    }

    void ReadyScenario(IScenario scenario)
    {
        Log("Scenario Ready : " + m_ScneraioName);
    }

    void UnLoad(IScenario scenario)
    {
        if(scenario != null)
        {
            Log("Scenario Pending Unload : " + m_ScneraioName);
            scenario.ScenarioStopCamera(() => StopCamera(scenario));
        }
    }

    void StopCamera(IScenario scenario)
    {
        if (scenario != null)
        {
            Log("Scenario Stop Camera : " + m_ScneraioName);
            scenario.ScenarioStop(() => StopScenario(scenario));
        }
    }

    void StopScenario(IScenario scenario)
    {
        if (scenario != null)
        {
            Log("Scenario Stop :" + m_ScneraioName);
            m_AcitveScenario = null;
            UnloadSceneAsync(m_ScneraioName);
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
