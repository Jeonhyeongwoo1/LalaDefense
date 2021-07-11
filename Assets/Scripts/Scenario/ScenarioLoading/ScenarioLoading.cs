using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScenarioLoading : MonoBehaviour, IScenario
{
    public LoadingAnimation loadingAnimation;

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
        loadingAnimation.StartLoadingAnimation();
        done?.Invoke();
    //    StartCoroutine(DelayCall(3f, GotoIntro));
    }

    IEnumerator DelayCall(float time, UnityAction done)
    {
        yield return new WaitForSeconds(time);
        done?.Invoke();
    }

    public void GotoIntro()
    {
        ScenarioDirector.Instance.OnLoadSceneAsync(nameof(ScenarioIntro));
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
        ScenarioDirector.Instance.OnLoaded(this, nameof(ScenarioLoading));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
