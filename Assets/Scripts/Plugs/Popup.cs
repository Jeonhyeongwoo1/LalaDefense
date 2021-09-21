using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BasePopup : MonoBehaviour
{
    public abstract void Open(UnityAction done);
    public abstract void Close(UnityAction done);
}

public class Popup : MonoBehaviour, IPlugable
{
    [SerializeField] List<BasePopup> popups = new List<BasePopup>();
    [SerializeField] List<BasePopup> m_Opened = new List<BasePopup>();

    public void RemoveOpenedPopup(BasePopup p)
    {
        m_Opened.Remove(p);
    }

    public T GetPopup<T>() where T : BasePopup
    {
        foreach (var v in popups)
        {
            T t = v as T;
            if (t != null) { return v.GetComponent<T>(); }
        }

        return null;
    }

    public bool IsOpenedPopup<T>() where T : BasePopup
    {
        foreach (var v in m_Opened)
        {
            T t = v as T;
            if (t != null) { return true; }
        }

        return false;
    }

    public List<BasePopup> GetOpenedPopups()
    {
        return m_Opened;
    }

    public void Open<T>(UnityAction done = null) where T : BasePopup
    {
        foreach (var v in popups)
        {
            T t = v as T;
            if (t != null)
            {
                BasePopup b = v.GetComponent<T>();
                
                if (!b.gameObject.activeSelf)
                {
                    b.gameObject.SetActive(true);
                }
                
                b.Open(done);
                m_Opened.Add(b);
            }
        }
    }

    public void Close<T>(UnityAction done = null) where T : BasePopup
    {
        foreach (var v in popups)
        {
            T t = v as T;
            if (t != null)
            {
                BasePopup b = v.GetComponent<T>();
                b.Close(done);
            }
        }
    }

    public void CloseOpenedPopups(UnityAction done = null)
    {
        if (m_Opened.Count == 0)
        {
            done?.Invoke();
            return;
        }

        StartCoroutine(ClosingOpenedPopups(done));
    }

    IEnumerator ClosingOpenedPopups(UnityAction done)
    {
        List<BasePopup> list = new List<BasePopup>();
        list.AddRange(m_Opened);

        for (int i = 0; i < list.Count; i++)
        {
            yield return Closing(list[i]);
        }

        done?.Invoke();
    }

    IEnumerator Closing(BasePopup popup)
    {
        bool isDone = false;
        popup.Close(() => isDone = true);
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
