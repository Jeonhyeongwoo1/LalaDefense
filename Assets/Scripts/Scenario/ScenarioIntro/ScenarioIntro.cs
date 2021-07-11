using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScenarioIntro : MonoBehaviour, IScenario
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
        StartCoroutine(DelayCall(3f, GotoHome));
    }

    public void ScenarioStopCamera(UnityAction done)
    {
        done?.Invoke();
    }

    public void ScenarioStop(UnityAction done)
    {
        done?.Invoke();
    }

    IEnumerator DelayCall(float time, UnityAction done)
    {
        yield return new WaitForSeconds(time);
        done?.Invoke();
    }

    void GotoHome()
    {
        //ScenarioDirector.Instace.OnLoadSceneAsync(nameof(ScenarioHome));
    }

    // Start is called before the first frame update
    void Start()
    {
        ScenarioDirector.Instance.OnLoaded(this, nameof(ScenarioIntro));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
