using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// Handles async and additive scene loading with optional loading screen support.
/// </summary>
public class SceneLoader : Singleton<SceneLoader>
{
    [Header("Optional")]
    [Tooltip("Name of the scene used as a loading screen.")]
    [SerializeField] private string loadingSceneName = "MyLoadingScene";

    /// <summary>
    /// Load a scene asynchronously. Optionally shows a loading screen.
    /// </summary>
    /// <param name="sceneName">The target scene to load.</param>
    /// <param name="useLoadingScene">Whether to load the loading scene first.</param>
    public void LoadScene(string sceneName, bool useLoadingScene = false)
    {
        StartCoroutine(LoadSceneAsync(sceneName, useLoadingScene));
    }

    /// <summary>
    /// Load a scene additively (without unloading the current scene).
    /// </summary>
    public void LoadSceneAdditive(string sceneName)
    {
        StartCoroutine(LoadSceneAdditiveAsync(sceneName));
    }

    /// <summary>
    /// Unload a previously loaded additive scene.
    /// </summary>
    public void UnloadScene(string sceneName)
    {
        if (SceneManager.GetSceneByName(sceneName).isLoaded)
        {
            SceneManager.UnloadSceneAsync(sceneName);
        }
    }

    private IEnumerator LoadSceneAsync(string targetScene, bool useLoading)
    {
        if (useLoading && !string.IsNullOrEmpty(loadingSceneName))
        {
            yield return SceneManager.LoadSceneAsync(loadingSceneName, LoadSceneMode.Single);
            yield return null; // Let the loading screen initialize
        }

        AsyncOperation async = SceneManager.LoadSceneAsync(targetScene, LoadSceneMode.Single);
        async.allowSceneActivation = false;

        while (!async.isDone)
        {
            // Optionally expose loading progress here (async.progress)
            if (async.progress >= 0.9f)
            {
                async.allowSceneActivation = true;
            }
            yield return null;
        }
    }

    private IEnumerator LoadSceneAdditiveAsync(string sceneName)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!async.isDone)
        {
            yield return null;
        }
    }
}
