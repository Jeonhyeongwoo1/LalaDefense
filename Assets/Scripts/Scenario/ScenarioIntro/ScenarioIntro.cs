using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using Cinemachine;

public class ScenarioIntro : MonoBehaviour, IScenario
{
    public string scenarioName => typeof(ScenarioIntro).Name;
    public PlayableDirector playableDirector;
    [SerializeField] CinemachineBrain brainCamera;
    [SerializeField] CinemachineVirtualCamera m_FrstCam;
    [SerializeField] float m_FadeOutDuration;

    public bool IsLive(ICinemachineCamera vcam) => CinemachineCore.Instance.IsLive(vcam) && brainCamera.IsLive(vcam);

    public void ScenarioPrepare(UnityAction done)
    {
        //카메라가 firstCam 위치에 있어야한다.
        BlockSkybox skybox = LalaStarter.GetBlockSkybox();
        skybox.FadeOut(1, () => done?.Invoke());
    }

    public void ScenarioStandbyCamera(UnityAction done)
    {
        //StartCoroutine(CameraTransistion(m_FrstCam, null));
        done?.Invoke();
    }

    public void ScenarioStart(UnityAction done)
    {
        playableDirector.Play();
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


    IEnumerator DelayCall(float time, UnityAction done)
    {
        yield return new WaitForSeconds(time);
        done?.Invoke();
    }

    //Signal
    public void GotoHome()
    {
        BlockSkybox skybox = LalaStarter.GetBlockSkybox();
        skybox.FadeIn(2, () => Core.Instance.scenario.OnLoadSceneAsync(nameof(ScenarioHome)));
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
