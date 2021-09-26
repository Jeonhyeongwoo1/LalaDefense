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

    string[] models =
    {
        nameof(HomeModel),
        nameof(Terrain)
    };

    string[] plugs =
    {
        nameof(Theme),
        nameof(Popup)
    };

    float m_TotalSceneCount;
    float m_LoadedSceneCount;

    public void ScenarioPrepare(UnityAction done)
    {
        m_TotalSceneCount = models.Length + plugs.Length;
        QualitySettings.SetQualityLevel(0);
        loadingAni.ScaleUpText(() => done?.Invoke());
    }

    public void ScenarioStandbyCamera(UnityAction done)
    {
        done?.Invoke();
    }

    public void ScenarioStart(UnityAction done)
    {
        StartCoroutine(Loading());
        StartCoroutine(LoadingModels());
        StartCoroutine(LoadingPlugs());
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
        foreach (string v in models)
        {
            yield return LoadingModelSceneAsync<IModel>(v, () => m_LoadedSceneCount++);
        }
    }

    IEnumerator LoadingPlugs()
    {
        foreach (string v in plugs)
        {
            yield return LoadingPlugsSceneAsync<IPlugable>(v, () => m_LoadedSceneCount++);
        }
    }

    IEnumerator LoadingModelSceneAsync<IModel>(string name, UnityAction done = null)
    {
        bool isDone = false;
        Core.models.OnLoadSceneAsync(name, () => isDone = true);
        while (isDone) { yield return null; }
        done?.Invoke();
    }

    IEnumerator LoadingPlugsSceneAsync<IPlugable>(string name, UnityAction done = null)
    {
        bool isDone = false;
        Core.plugs.OnLoadSceneAsync(name, () => isDone = true);
        while (isDone) { yield return null; }
        done?.Invoke();
    }

    IEnumerator DelayCall(float time, UnityAction done)
    {
        yield return new WaitForSeconds(time);
        done?.Invoke();
    }

    IEnumerator Loading()
    {
        AnimationCurve ac = loadingAni.m_NormalCurve;
        float axisX = 0f;
        float elapsed = 0;
        float duration = 0;
        while (loadingAni.slider.value != 1)
        {
            elapsed += Time.deltaTime;
            duration = Mathf.Min(elapsed / m_MinLoadingDuration, m_LoadedSceneCount / m_TotalSceneCount);
            loadingAni.slider.value = Mathf.Lerp(0, 1, duration);
            axisX = Mathf.Lerp(0, 1, duration);
            loadingAni.wave.uvRect = new Rect(new Vector2(axisX, 0), Vector2.one);
            yield return null;
        }

        // Go to Intro
        GotoIntro();
    }

    public void GotoIntro()
    {
        BlockSkybox skybox = LalaStarter.GetBlockSkybox();
        skybox.FadeIn(fadeDuration, () => Core.scenario.OnLoadSceneAsync(nameof(ScenarioIntro)));
    }

    // Start is called before the first frame update
    void Start()
    {
        Core.Ensure(() => Core.scenario.OnLoaded(this));
    }

}
