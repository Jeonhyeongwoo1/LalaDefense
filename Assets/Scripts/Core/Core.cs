using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : Singleton<Core>
{
    [SerializeField] ScenarioDirector scenarioDirector;
    [SerializeField] ModelDirector modelDirector;
    [SerializeField] PlugDirector plugDirector;

    public ScenarioDirector scenario => scenarioDirector;
    public ModelDirector model => modelDirector;
    public PlugDirector plug => plugDirector;

    void Default()
    {
        
    }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        Default();
    }

}
