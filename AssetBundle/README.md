### AssetBundle
A basic class to build AssetBundles and create a JSON hash list for later download.
[AssetBundles - Unity Manual](https://docs.unity3d.com/Manual/AssetBundlesIntro.html)

> **Note:** Unity now provides [Addressables](https://docs.unity3d.com/Packages/com.unity.addressables@latest) as a high-level API to manage remote content. It is recommended to use Addressables instead of handling AssetBundles manually in most cases.

#### Setup Instructions

1. **Create a GameObject**  
   - Add a new GameObject in your scene.  
   - Attach the `BundleDownloader.cs` script to it.

2. **Assign AssetBundles**  
   - Select assets (e.g., textures, prefabs, scenes) in the Project window.  
   - In the Inspector, set their **AssetBundle** name.

3. **Export Bundles and Hashes**  
   - From the top menu, go to:  
     `Sema-Tools > Export Bundle & Hashes`  
   - This will export bundles and generate a `bundle_hashes.json`.

4. **Upload to Server/CDN**  
   - Upload the generated bundles and `bundle_hashes.json` to your web host or CDN.

5. **Set Base URL**  
   - In the inspector, set the `bundlesBaseUrl` to point to your uploaded bundles directory.
