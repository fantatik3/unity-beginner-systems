using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;
using System.Collections.Generic;

public class BundleDownloader : MonoBehaviour
{

    [SerializeField] private string bundlesBaseUrl = "https://your_CDN_Url.com/assetbundles/";
    [SerializeField] private string bundleJsonName = "bundle_hashes.json";
    [SerializeField] private BundleDownloadStatusUI ui;

    //For Testing purposes
    [SerializeField] private bool clearCache = false;
    [Serializable]
    public class BundleHashEntry
    {
        public string name;
        public string hash;
    }

    [Serializable]
    public class BundleHashList
    {
        public List<BundleHashEntry> bundles;
    }

    private void Start()
    {
        if (clearCache && Caching.ClearCache())
            Debug.Log("Cache cleared successfully.");
        
        if (ui != null)
            ui.ResetUI();

        StartCoroutine(DownloadAndLoadBundles());
    }

    IEnumerator DownloadAndLoadBundles()
    {
        string jsonUrl = bundlesBaseUrl + bundleJsonName;
        UnityWebRequest jsonRequest = UnityWebRequest.Get(jsonUrl);
        yield return jsonRequest.SendWebRequest();

        if (jsonRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Failed to download hash JSON: " + jsonRequest.error);
            if (ui != null)
                ui.SetStatus("Failed to load bundle list.");
            yield break;
        }

        string jsonText = jsonRequest.downloadHandler.text;
        BundleHashList bundleList = JsonUtility.FromJson<BundleHashList>(jsonText);

        foreach (var bundle in bundleList.bundles)
        {
            string fullUrl = bundlesBaseUrl + bundle.name;
            Hash128 hash = Hash128.Parse(bundle.hash);

            if (Caching.IsVersionCached(fullUrl, hash))
                Debug.Log("Bundle is cached: " + bundle.name);
            else
                Debug.Log("Downloading bundle: " + bundle.name);

            if (ui != null)
            {
                ui.SetStatus("Downloading: " + bundle.name);
                ui.SetProgress(0f);
            }

            UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(fullUrl, hash, 0);
            var operation = request.SendWebRequest();

            while (!operation.isDone)
            {
                if (ui != null)
                {
                    ui.SetProgress(operation.progress);
                }
                yield return null;
            }

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Failed to download bundle: " + bundle.name + " - " + request.error);
                if (ui != null)
                    ui.SetStatus("Failed: " + bundle.name);
                continue;
            }
            if (ui != null)
            {
                ui.SetStatus("Completed: " + bundle.name);
                ui.SetProgress(1f);
            }

            yield return new WaitForSeconds(0.5f);
        }

        if (ui != null)
            ui.SetStatus("All bundles downloaded.");
    }
}
