using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScenarioPlay : MonoBehaviour, IScenario
{
    public string scenarioName => typeof(ScenarioPlay).Name;

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


    // Start is called before the first frame update
    void Start()
    {
        ScenarioDirector.Instance.OnLoaded(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
