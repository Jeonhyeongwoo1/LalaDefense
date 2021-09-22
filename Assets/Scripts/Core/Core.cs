using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Core : Singleton<Core>
{
    static public ScenarioDirector scenario => Core.Instance?.m_ScenarioDirector;
    static public ModelDirector models => Core.Instance?.m_ModelDirector;
    static public PlugDirector plugs => Core.Instance?.m_PlugDirector;
    static public GamePlayManager gameManager => Core.Instance?.m_GamePlayManager;
    static public XState state => Core.Instance?.m_XState;

    [SerializeField] GamePlayManager m_GamePlayManager = null;
    [SerializeField] ScenarioDirector m_ScenarioDirector = null;
    [SerializeField] ModelDirector m_ModelDirector = null;
    [SerializeField] PlugDirector m_PlugDirector = null;
    [SerializeField] XState m_XState = null;

    static UnityEvent m_EnsureDone = new UnityEvent();

    // static methods
    static public void Ensure(UnityAction done = null)
    {
        if (Core.Instance)
        {
            done?.Invoke();
            return;
        }

        if (done != null) { m_EnsureDone.AddListener(done); }

        Scene coreScene = SceneManager.GetSceneByName(nameof(Core));
        if (coreScene == null || !coreScene.IsValid())
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene(nameof(Core), LoadSceneMode.Additive);
        }
    }

    static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != nameof(Core)) { return; }

        SceneManager.sceneLoaded -= OnSceneLoaded;
        Debug.Log("XCores Scene loaded");
        m_EnsureDone.Invoke();
        m_EnsureDone.RemoveAllListeners();
    }

    X EnsureCore<X>(ref X behaviour) where X : MonoBehaviour
    {
        if (!behaviour) { behaviour = Core.Instance.CreateCore<X>(); }

        return behaviour as X;
    }

    // public methods
    private void Awake()
    {
        EnsureAllCores();
    }

    public void EnsureAllCores()
    {
        EnsureCore<ScenarioDirector>(ref m_ScenarioDirector);
        EnsureCore<ModelDirector>(ref m_ModelDirector);
        EnsureCore<PlugDirector>(ref m_PlugDirector);
        EnsureCore<GamePlayManager>(ref m_GamePlayManager);
        EnsureCore<XState>(ref m_XState);

        Debug.Log("Core Initialized.");
    }

    T CreateCore<T>() where T : MonoBehaviour
    {
        var v = new GameObject(typeof(T).Name);
        v.transform.SetParent(this.transform);
        return v.AddComponent<T>();
    }
}