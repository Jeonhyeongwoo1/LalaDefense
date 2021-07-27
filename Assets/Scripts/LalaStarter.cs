using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        //blockSkybox.FadeOut(1, null);
        //LoadScenarioLoading();
    }

    void LoadScenarioLoading()
    {
        ScenarioDirector.Instance.OnLoadSceneAsync(nameof(ScenarioLoading));
    }

}
