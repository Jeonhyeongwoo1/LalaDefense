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
        StartCoroutine(CoUtilize.VLerp((v) => m_Start.transform.localScale = v, Vector3.zero, Vector3.one, 0.3f, () => done?.Invoke(), null));
    }

    public void ScenarioStopCamera(UnityAction done)
    {
        done?.Invoke();
    }

    public void ScenarioStop(UnityAction done)
    {
        StartCoroutine(CoUtilize.VLerp((v) => m_Start.transform.localScale = v, Vector3.one, Vector3.zero, 0.3f, () => done?.Invoke()));
    }

    void OnGameStart()
    {
        Popup popup = FindObjectOfType<Popup>();
        popup.stagePopup.gameObject.SetActive(true);
        popup.stagePopup.Open(null);
        //Core.Instance.scenario.OnLoadSceneAsync(nameof(ScenarioPlay));
    }

    // Start is called before the first frame update
    void Start()
    {
        ScenarioDirector.Instance.OnLoaded(this);
//        Core.Instance.scenario.OnLoaded(this);
        m_Start.onClick.AddListener(OnGameStart);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            Popup popup = FindObjectOfType<Popup>();
            popup.pausePopup.gameObject.SetActive(true);
            popup.pausePopup.Open(null);
        }
    }
}
