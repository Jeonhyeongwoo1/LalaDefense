using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseTheme : MonoBehaviour
{
    public abstract void Open(UnityAction done);
    public abstract void Close(UnityAction done);
}

public class Theme : MonoBehaviour, IPlugable
{
    [SerializeField] List<BaseTheme> themes = new List<BaseTheme>();
    [SerializeField] List<BaseTheme> m_Opened = new List<BaseTheme>();

    public void RemoveOpenedTheme(BaseTheme p)
    {
        m_Opened.Remove(p);
    }

    public T GetTheme<T>() where T : BaseTheme
    {
        foreach (var v in themes)
        {
            T t = v as T;
            if (t != null) { return v.GetComponent<T>(); }
        }

        return null;
    }

    public bool IsOpenedTheme<T>() where T : BaseTheme
    {
        foreach (var v in m_Opened)
        {
            T t = v as T;
            if (t != null) { return true; }
        }

        return false;
    }

    public List<BaseTheme> GetOpenedThemes()
    {
        return m_Opened;
    }

    public void Open<T>(UnityAction done = null) where T : BaseTheme
    {
        foreach (var v in themes)
        {
            T t = v as T;
            if (t != null)
            {
                BaseTheme b = v.GetComponent<T>();

                if (!b.gameObject.activeSelf)
                {
                    b.gameObject.SetActive(true);
                }

                b.Open(done);
                m_Opened.Add(b);
            }
        }
    }

    public void Close<T>(UnityAction done = null) where T : BaseTheme
    {
        foreach (var v in themes)
        {
            T t = v as T;
            if (t != null)
            {
                BaseTheme b = v.GetComponent<T>();
                b.Close(done);
            }
        }
    }

    public void CloseOpenedThemes(UnityAction done = null)
    {
        if (m_Opened.Count == 0)
        {
            done?.Invoke();
            return;
        }
        
        StartCoroutine(ClosingOpenedThemes(done));
    }

    IEnumerator ClosingOpenedThemes(UnityAction done)
    {
        List<BaseTheme> list = new List<BaseTheme>();
        list.AddRange(m_Opened);

        for(int i = 0; i < list.Count; i++)
        {
            yield return Closing(list[i]);
        }

        done?.Invoke();
    }

    IEnumerator Closing(BaseTheme theme)
    {
        bool isDone = false;
        theme.Close(() => isDone = true);
        while (!isDone) { yield return null; }
    }


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        Core.Ensure(() => Core.plugs.SetPlug(this));
    }

}
