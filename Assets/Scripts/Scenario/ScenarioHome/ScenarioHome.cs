using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ScenarioHome : MonoBehaviour, IScenario
{
    public string scenarioName => typeof(ScenarioHome).Name;
    [SerializeField] Button m_Start;
    
    public void ScenarioPrepare(UnityAction done)
    {
        BlockSkybox skybox = LalaStarter.GetBlockSkybox();
        skybox.FadeOut(2, () => done?.Invoke());
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

    void OnGameStart()
    {
        ScenarioDirector.Instance.OnLoadSceneAsync(nameof(ScenarioPlay));
    }

    // Start is called before the first frame update
    void Start()
    {
        ScenarioDirector.Instance.OnLoaded(this);
        m_Start.onClick.AddListener(OnGameStart);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
