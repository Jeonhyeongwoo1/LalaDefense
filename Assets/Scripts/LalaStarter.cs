using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LalaStarter : MonoBehaviour
{
    public BlockSkybox blockSkybox;

    public static BlockSkybox GetBlockSkybox()
    {
        return FindObjectOfType<BlockSkybox>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // LoadCore();

        //blockSkybox.FadeOut(1, null);
        //LoadScenarioLoading();
    }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        Core.Ensure(LoadScenarioLoading);
    }
    
    void LoadScenarioLoading()
    {
        //Core.scenario.OnLoadSceneAsync(nameof(ScenarioLoading));
    }

}
