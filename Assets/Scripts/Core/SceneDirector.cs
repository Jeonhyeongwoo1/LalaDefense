using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public interface IPlayable
{

}

public class SceneDirector<XType> : Singleton<SceneDirector<XType>> where XType : IPlayable
{
    private List<XType> XTypes = new List<XType>();
    public UnityAction<Scene, LoadSceneMode> sceneLoaded;

    bool HasXType(XType type)
    {
        XType x = XTypes.Find((v) => type.Equals(v));
        return x != null ? true : false;
    }

    public XType GetType(XType type) { return XTypes.Find((v) => type.Equals(v)); }

    public void Ensure(object scene, UnityAction done = null)
    {
        // bool isXType = scene is IPlayable;
        // if (!isXType) { return; }

        if (HasXType((XType) scene)) { return; }
     
        StartCoroutine(LoadingSceneAsync(nameof(scene), done));
    }

    public void OnLoadSceneAsync(object sceneName, UnityAction done)
    {
        StartCoroutine(LoadingSceneAsync(sceneName, done));
    }

    public void UnloadSceneAsync(object sceneName, UnityAction done)
    {
        StartCoroutine(UnloadingSceneAsync(sceneName, done));
    }

    public void ProgressLoadSceneAsync(object sceneName, UnityAction<float> progress, UnityAction done)
    {
        StartCoroutine(ProgressLoadingSceneAsync(sceneName, done, (v) => progress.Invoke(v)));
    }

    public void ProgressUnloadSceneAsync(object sceneName, UnityAction<float> progress, UnityAction done)
    {
        StartCoroutine(ProgressLoadingSceneAsync(sceneName, done, (v) => progress.Invoke(v)));
    }

    // 0.9 up
    IEnumerator ProgressUnLoadingSceneAsync(object sceneName, UnityAction done, UnityAction<float> progress)
    {
        AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(nameof(sceneName));
        while (!asyncOperation.isDone)
        {
            progress.Invoke(asyncOperation.progress);
            yield return null;
        }
        XTypes.Add((XType)sceneName);
        done?.Invoke();
    }

    // 0.9 up
    IEnumerator ProgressLoadingSceneAsync(object sceneName, UnityAction done, UnityAction<float> progress)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(nameof(sceneName), LoadSceneMode.Additive);
        while (!asyncOperation.isDone)
        {
            progress.Invoke(asyncOperation.progress);
            yield return null;
        }
        XTypes.Remove((XType)sceneName);
        done?.Invoke();
    }

    IEnumerator LoadingSceneAsync(object sceneName, UnityAction done)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(nameof(sceneName), LoadSceneMode.Additive);
        while (!asyncOperation.isDone) { yield return null; }
        XTypes.Add((XType)sceneName);
        done?.Invoke();
    }

    IEnumerator UnloadingSceneAsync(object sceneName, UnityAction done)
    {
        AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(nameof(sceneName));
        while (!asyncOperation.isDone) { yield return null; }
        XTypes.Remove((XType)sceneName);
        done?.Invoke();
    }

}
