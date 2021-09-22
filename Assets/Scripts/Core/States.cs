using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void State(string key, object o);

public interface IState
{
    void Notify(string key, object o);
}

public class States : MonoBehaviour
{
    protected Dictionary<string, StateObserver> observers = new Dictionary<string, StateObserver>();

    public class StateObserver
    {
        event State observer;
        object stateValue;

        public void Watch(string key, State call)
        {
            if (key == null) { return; }

            observer += call;
        }

        public void Stop(string key, State call)
        {
            observer -= call;
        }

        public void Set(string key, object o)
        {
            if (key == null) { return; }
            stateValue = o;
            observer?.Invoke(key, o);
        }

        public object Get() => stateValue;
    }

    StateObserver GetObserver(string key)
    {
        StateObserver stateObserver = null;
        if (!observers.TryGetValue(key, out stateObserver))
        {
            stateObserver = new StateObserver();
            observers.Add(key, stateObserver);
        }

        return stateObserver;
    }

    public void SetState(string key, object o)
    {
        GetObserver(key)?.Set(key, o);
    }

    public void Watch(string key, State call)
    {
        GetObserver(key)?.Watch(key, call);
    }

    public void Stop(string key, State call)
    {
        GetObserver(key)?.Stop(key, call);
    }

    public object Get(string key)
    {
        return GetObserver(key)?.Get();
    }

    public void RemoveAll(string key)
    {
        observers.Remove(key);
    }

}
