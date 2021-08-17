using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScenarioLoading : MonoBehaviour, IScenario
{
    public string scenarioName => typeof(ScenarioLoading).Name;
    public LoadingAnimation loadingAni;

    [Range(0, 5), SerializeField] float m_MinLoadingDuration = 4f;
    [SerializeField] float fadeDuration = 2f;

    private object[] models =
    {
        nameof(Terrain),
        nameof(HomeModel)
    };

    private object[] plugs =
    {
        nameof(Theme),
        nameof(Popup)
    };

    public void ScenarioPrepare(UnityAction done)
    {
        loadingAni.ScaleUpText(() => done?.Invoke());
    }

    public void ScenarioStandbyCamera(UnityAction done)
    {
        done?.Invoke();
    }

    public void ScenarioStart(UnityAction done)
    {
        StartCoroutine(ElapsedTime());
        StartCoroutine(Loading());
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

    IEnumerator LoadingModels()
    {
        foreach (IModel v in models)
        {
            yield return LoadingSceneAsync<IModel>(v);
        }
    }

    IEnumerator LoadingPlugs()
    {
        foreach (IPlug v in plugs)
        {
            yield return LoadingSceneAsync<IPlug>(v);
        }
    }

    IEnumerator LoadingSceneAsync<T>(object name, UnityAction done = null) where T : IPlayable
    {
        bool isDone = false;
        // SceneDirector<T>.Instance.OnLoadSceneAsync(name, () => isDone = true);
        while (isDone) { yield return null; }
        done?.Invoke();
    }

    IEnumerator DelayCall(float time, UnityAction done)
    {
        yield return new WaitForSeconds(time);
        done?.Invoke();
    }

    float m_ElapsedLoadingTime = 0;
    IEnumerator ElapsedTime()
    {
        while (m_ElapsedLoadingTime < m_MinLoadingDuration)
        {
            m_ElapsedLoadingTime += Time.deltaTime;
            yield return null;
        }
    }

    //Temporary
    IEnumerator Loading()
    {
        AnimationCurve ac = loadingAni.m_NormalCurve;
        float axisX = 0f;

        while (m_ElapsedLoadingTime < m_MinLoadingDuration)
        {
            loadingAni.slider.value = Mathf.Lerp(0, 1, ac.Evaluate(m_ElapsedLoadingTime / m_MinLoadingDuration));
            axisX = Mathf.Lerp(0, m_ElapsedLoadingTime, ac.Evaluate(m_ElapsedLoadingTime / m_MinLoadingDuration));
            loadingAni.wave.uvRect = new Rect(new Vector2(axisX, 0), Vector2.one);
            yield return null;
        }

        // Go to Intro
        GotoIntro();
    }

    public void GotoIntro()
    {
        BlockSkybox skybox = LalaStarter.GetBlockSkybox();
        skybox.FadeIn(fadeDuration, () => Core.Instance.scenario.OnLoadSceneAsync(nameof(ScenarioIntro)));
    }

    // Start is called before the first frame update
    void Start()
    {
        Core.Instance.scenario.OnLoaded(this);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
