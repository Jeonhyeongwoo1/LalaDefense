using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LalaStarter : MonoBehaviour
{
    public static BlockSkybox GetBlockSkybox()
    {
        return FindObjectOfType<BlockSkybox>();
    }

    void Awake()
    {
        Core.Ensure(LoadScenarioLoading);
    }
    
    void LoadScenarioLoading()
    {
        Core.scenario.OnLoadSceneAsync(nameof(ScenarioLoading));
    }

}
