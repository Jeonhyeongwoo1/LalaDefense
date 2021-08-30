using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HomeModel : MonoBehaviour, IModel
{
    public string modelName => typeof(HomeModel).Name;

    public void Open(UnityAction done)
    {
        gameObject.SetActive(true);
        done?.Invoke();
    }

    public void Close(UnityAction done)
    {
        done?.Invoke();
        gameObject.SetActive(false);
    }
    
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        Core.Ensure(() => Core.models.SetModel(this));
    }


}
