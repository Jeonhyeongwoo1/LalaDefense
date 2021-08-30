using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public interface IPlugable : IPlayable
{

}

public class PlugDirector : SceneDirector<IPlugable>
{
    List<IPlugable> plugables = new List<IPlugable>();

    public IPlugable GetPlugable(IPlugable plugable)
    {
        return plugables.Find((v) => v == plugable);
    }

    public X GetPlugable<X>() where X : MonoBehaviour, IPlugable
    {
        foreach (var v in plugables)
        {
            X x = v as X;
            if (x != null) { return x; }
        }

        return null;
    }

    public void SetPlug(IPlugable plugable)
    {
        plugables.Add(plugable);
    }

}
