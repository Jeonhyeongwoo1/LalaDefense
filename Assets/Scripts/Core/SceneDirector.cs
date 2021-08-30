using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public interface IPlayable
{

}

public class SceneDirector<XType> : MonoBehaviour where XType : IPlayable
{
    public UnityAction<Scene, LoadSceneMode> sceneLoaded;

    public void Ensure<T>(UnityAction done = null) where T : XType
    {
        string name = typeof(T).ToString();

        Scene scene = SceneManager.GetSceneByName(name);
        if (scene.isLoaded) { return; }

        StartCoroutine(LoadingSceneAsync(name, done));
    }

    public void OnLoadSceneAsync(string sceneName, UnityAction done)
    {
        Scene scene = SceneManager.GetSceneByName(sceneName);
        if (scene.isLoaded) { return; }

        StartCoroutine(LoadingSceneAsync(sceneName, done));
    }

    public void UnloadSceneAsync(string sceneName, UnityAction done)
    {
        Scene scene = SceneManager.GetSceneByName(sceneName);
        if (!scene.isLoaded) { return; }

        StartCoroutine(UnloadingSceneAsync(sceneName, done));
    }

    public void ProgressLoadSceneAsync(string sceneName, UnityAction<float> progress, UnityAction done)
    {
        Scene scene = SceneManager.GetSceneByName(sceneName);
        if (scene.isLoaded) { return; }
        
        StartCoroutine(ProgressLoadingSceneAsync(sceneName, done, (v) => progress.Invoke(v)));
    }

    public void ProgressUnloadSceneAsync(string sceneName, UnityAction<float> progress, UnityAction done)
    {
        Scene scene = SceneManager.GetSceneByName(sceneName);
        if (!scene.isLoaded) { return; }

        StartCoroutine(ProgressLoadingSceneAsync(sceneName, done, (v) => progress.Invoke(v)));
    }

    // 0.9 up
    IEnumerator ProgressUnLoadingSceneAsync(string sceneName, UnityAction done, UnityAction<float> progress)
    {
        AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(sceneName);
        while (!asyncOperation.isDone)
        {
            progress.Invoke(asyncOperation.progress);
            yield return null;
        }
        done?.Invoke();
    }

    // 0.9 up
    IEnumerator ProgressLoadingSceneAsync(string sceneName, UnityAction done, UnityAction<float> progress)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!asyncOperation.isDone)
        {
            progress.Invoke(asyncOperation.progress);
            yield return null;
        }
        done?.Invoke();
    }

    IEnumerator LoadingSceneAsync(string sceneName, UnityAction done)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!asyncOperation.isDone) { yield return null; }
        done?.Invoke();
    }

    IEnumerator UnloadingSceneAsync(string sceneName, UnityAction done)
    {
        AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(sceneName);
        while (!asyncOperation.isDone) { yield return null; }
        done?.Invoke();
    }

}
