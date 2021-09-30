using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;
using UnityEngine.UI;

public class ScenarioHome : MonoBehaviour, IScenario
{
    public string scenarioName => typeof(ScenarioHome).Name;
    [SerializeField] Button m_Start;
    [SerializeField] CinemachineVirtualCamera m_HomeCam;
    [SerializeField] BgmOnOff m_BgmOnOff;

    bool IsLive(CinemachineVirtualCamera cam) => CinemachineCore.Instance.IsLive(cam) && !CinemachineCore.Instance.GetActiveBrain(0).IsBlending;

    public void ScenarioPrepare(UnityAction done)
    {
        QualitySettings.SetQualityLevel(0);
        Core.models.DefaultLoadModels();
        m_BgmOnOff.ChangeImage(!Core.state.mute ? true : false);
        Core.models.GetModel<HomeModel>()?.Open(null); 
        m_Start.onClick.AddListener(OnGameStart);
        done?.Invoke();
    }

    public void ScenarioStandbyCamera(UnityAction done)
    {
        StartCoroutine(CameraTransistion(m_HomeCam, done));
    }

    public void ScenarioStart(UnityAction done)
    {
        m_BgmOnOff.isOn = !Core.state.mute ? true : false;
        Core.models.GetModel<Terrain>()?.Close(null);
        BlockSkybox skybox = LalaStarter.GetBlockSkybox();
        skybox.FadeOut(1, () =>
        {
            OpenStartBtn();
            done?.Invoke();
        });

    }

    public void ScenarioStopCamera(UnityAction done)
    {
        done?.Invoke();
    }

    public void ScenarioStop(UnityAction done)
    {
        if (m_BgmOnOff.isOn)
        {
            m_BgmOnOff.isOn = false;
        }

        done?.Invoke();
    }

    void OnGameStart()
    {
        Popup popup = Core.plugs.GetPlugable<Popup>();
        popup.Open<StagePopup>();
        popup.GetPopup<StagePopup>().stageReadyPopup.closeEvent = () =>
        {
            m_BgmOnOff.isOn = false;
            m_BgmOnOff.gameObject.SetActive(false);
            m_Start.gameObject.SetActive(false);
        };
    }

    void OpenStartBtn()
    {
        StartCoroutine(CoUtilize.VLerp((v) => m_Start.transform.localScale = v, Vector3.zero, Vector3.one, 0.3f));
    }

    IEnumerator CameraTransistion(CinemachineVirtualCamera cam, UnityAction done)
    {
        while (!IsLive(cam)) { yield return null; }
        done?.Invoke();
    }

    // Start is called before the first frame update
    void Start()
    {
        Core.Ensure(() => Core.scenario.OnLoaded(this));
    }
}
