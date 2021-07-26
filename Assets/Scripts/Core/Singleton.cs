using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance = null;
    private static readonly object paylock = new object();

    public static T Instance
    {
        get
        {
            lock (paylock)
            {
                if (instance == null)
                {
                    instance = new GameObject(typeof(T).ToString(), typeof(T)).GetComponent<T>();
                    Debug.Log("Create Singleton : " + instance.name);
                    return instance;
                }
                else
                {
                    return instance;
                }

            }
        }
    }
}
