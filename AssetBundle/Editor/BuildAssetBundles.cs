using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class AssetBundleHashExporter
{
    [MenuItem("Sema-Tools/Export Bundle & Hashes")]
    public static void ExportBundleHashes()
    {
        string bundleDir = "AssetBundles";
        string outputJson = Path.Combine(bundleDir, "bundle_hashes.json");
        var manifest = AssetBundle.LoadFromFile(Path.Combine(bundleDir, "AssetBundles"));
        var abManifest = manifest.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

        var dict = new Dictionary<string, string>();

        foreach (string bundleName in abManifest.GetAllAssetBundles())
        {
            Hash128 hash = abManifest.GetAssetBundleHash(bundleName);
            dict[bundleName] = hash.ToString();
        }

        File.WriteAllText(outputJson, JsonUtility.ToJson(new BundleHashList(dict)));
        Debug.Log("Bundle hashes exported to: " + outputJson);
    }

    [System.Serializable]
    public class BundleHashList
    {
        public List<BundleHash> bundles = new();

        public BundleHashList(Dictionary<string, string> dict)
        {
            foreach (var kv in dict)
                bundles.Add(new BundleHash { name = kv.Key, hash = kv.Value });
        }
    }

    [System.Serializable]
    public class BundleHash
    {
        public string name;
        public string hash;
    }
}
