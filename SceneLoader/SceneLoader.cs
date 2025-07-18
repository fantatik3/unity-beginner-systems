using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

    [SerializeField] private string loadingSceneName = "MyScene";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Loads a scene asynchronously with optional loading screen.
    /// </summary>
    /// <param name="sceneName">Target scene name</param>
    /// <param name="useLoadingScene">Whether to show the loading scene</param>
    public void LoadScene(string sceneName, bool useLoadingScene = false)
    {
        StartCoroutine(LoadSceneAsync(sceneName, useLoadingScene));
    }

    /// <summary>
    /// Loads a scene additively (e.g., for multi-scene setups).
    /// </summary>
    public void LoadSceneAdditive(string sceneName)
    {
        StartCoroutine(LoadSceneAdditiveAsync(sceneName));
    }

    /// <summary>
    /// Unloads an additive scene.
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
        if (useLoading)
        {
            yield return SceneManager.LoadSceneAsync(loadingSceneName, LoadSceneMode.Single);
            yield return null; // Let loading scene initialize
        }

        AsyncOperation async = SceneManager.LoadSceneAsync(targetScene, LoadSceneMode.Single);
        async.allowSceneActivation = false;

        while (!async.isDone)
        {
            // You can hook into async.progress here (0–0.9 is loading, 0.9–1 is activation)
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
