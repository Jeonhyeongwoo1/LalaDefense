using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using Cinemachine;
using UnityEngine.UI;

public class ScenarioIntro : MonoBehaviour, IScenario
{
    public string scenarioName => typeof(ScenarioIntro).Name;
    public PlayableDirector playableDirector;

    [SerializeField] Button m_Skip;
    [SerializeField] CinemachineBrain m_BrainCamera;
    [SerializeField] CinemachineVirtualCamera m_FrstCam;
    [SerializeField] float m_FadeOutDuration;
    Camera m_MainCamera;
    bool IsLive(CinemachineVirtualCamera cam) => CinemachineCore.Instance.IsLive(cam) && !CinemachineCore.Instance.GetActiveBrain(0).IsBlending;

    public void ScenarioPrepare(UnityAction done)
    {
        Core.models.DefaultLoadModels();
        QualitySettings.SetQualityLevel(0);
        m_MainCamera = Camera.main;
        m_MainCamera.gameObject.SetActive(false);
        Core.models.GetModel<Terrain>()?.Close(null);
        Core.models.GetModel<HomeModel>()?.Close(null);
        m_Skip.onClick.AddListener(OnSkipIntro);
        done?.Invoke();
    }

    public void ScenarioStandbyCamera(UnityAction done)
    {
        StartCoroutine(CameraTransistion(m_FrstCam, done));
    }

    public void ScenarioStart(UnityAction done)
    {
        BlockSkybox skybox = LalaStarter.GetBlockSkybox();
        skybox.FadeOut(1, () =>
        {
            done?.Invoke();
            playableDirector.Play();
            m_Skip.gameObject.SetActive(true);
        });

    }

    public void ScenarioStopCamera(UnityAction done)
    {
        done?.Invoke();
    }

    public void ScenarioStop(UnityAction done)
    {
        m_MainCamera.gameObject.SetActive(true);
        done?.Invoke();
    }

    IEnumerator CameraTransistion(CinemachineVirtualCamera cam, UnityAction done)
    {
        cam.enabled = false;
        cam.enabled = true;
        while (!IsLive(cam)) { yield return null; }
        done?.Invoke();
    }

    void OnSkipIntro()
    {
        playableDirector.stopped += (PlayableDirector b) => GotoHome();
        playableDirector.Stop();
    }

    //Signal
    public void GotoHome()
    {
        BlockSkybox skybox = LalaStarter.GetBlockSkybox();
        skybox.FadeIn(2, () => Core.scenario.OnLoadSceneAsync(nameof(ScenarioHome)));
    }

    // Start is called before the first frame update
    void Start()
    {
        Core.Ensure(() => Core.scenario.OnLoaded(this));
    }

    IEnumerator BlendingCamera(UnityAction done)
    {
        while (CinemachineCore.Instance.GetActiveBrain(0).IsBlending) { yield return null; }
        done.Invoke();
    }

}
