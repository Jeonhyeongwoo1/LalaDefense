using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    void LoadCore()
    {
        SceneManager.LoadScene(nameof(Core), LoadSceneMode.Additive);
    }

    void LoadScenarioLoading()
    {
        ScenarioDirector.Instance.OnLoadSceneAsync(nameof(ScenarioLoading));
    }

}
