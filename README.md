

# Unity Helpers

A growing collection of simple, reusable systems and tools for Unity designed to help **beginners** and **solo devs** get up and running faster.
Whether you're just starting out or need quick plug-and-play solutions, Unity Helpers provides lightweight, cleanly written components you can drop into any project.


## Index

- [AssetBundle](#assetbundle)
- [Singleton\<T\>](#singletont)
- [SceneLoader](#sceneloader)
- [LocalizationManager](#localizationmanager)
- [InteractableObjects](#interactableobjects)
---

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
   
---

### Singleton\<T\>

A generic base class for creating globally accessible, persistent MonoBehaviour singletons.

This pattern is useful for systems like `AudioManager`, `SceneLoader`, `GameManager`, etc., where you need a **single instance** that's accessible from anywhere and survives scene changes.

####  Features

- Ensures only one instance exists.
- Automatically creates an instance if none exists.
- Persists across scene loads (`DontDestroyOnLoad`).
- Cleans up automatically on application quit or destruction.
- Easy to extend in your own MonoBehaviours.

#### Usage

##### 1. Inherit from `Singleton<T>`

```csharp
public class AudioManager : Singleton<AudioManager>
{
    protected override void Awake()
    {
        base.Awake(); // IMPORTANT: Always call base.Awake()

        // Your initialization logic
    }

    public void PlaySound(string name)
    {
        Debug.Log($"Playing sound: {name}");
    }
}
```
##### 2. Access the instance anywhere

```csharp
AudioManager.Instance.PlaySound("Click");
```
---
### SceneLoader

A lightweight, reusable scene loading utility for Unity that supports asynchronous scene loading, additive scene loading, and optional loading screen integration.

#### Features

- Load scenes asynchronously with progress handling.
- Use a dedicated loading screen while loading other scenes.
- Load scenes additively (for multi-scene setups).
- Unload additive scenes safely.
- Singleton-based and persistent across scenes.

### Usage:

#### Basic Setup

1. Add the `SceneLoader.cs` script to a GameObject in your starting scene.
2. (Optional) Create a new scene and assign the name to `loadingSceneName` if using a loading screen.
3. (Optional) Add the scene to the project build.
4. Mark the GameObject as `DontDestroyOnLoad` if not using singleton.

#### Load a Scene with optional loading screen (Manual References - Recommended to use Singleton)

```csharp
// Reference SceneLoader
[SerializeField] private SceneLoader sceneLoader;

// Load "MainMenu" scene with a loading screen
sceneLoader.LoadScene("MainMenu", useLoadingScene: true);

// Load "Level1" scene directly
sceneLoader.LoadScene("Level1");
```

#### Load a Scene with optional loading screen (Singleton)

```csharp
// Load "MainMenu" scene with a loading screen
SceneLoader.Instance.LoadScene("MainMenu", useLoadingScene: true);

// Load "Level1" scene directly
SceneLoader.Instance.LoadScene("Level1");
```
---

### LocalizationManager

A minimal, extensible localization system for Unity supporting multiple languages via structured JSON files.

#### Features

- Loads language-specific JSON files from `StreamingAssets/Localization/`.
- Fast key-based lookup with section support: `Translate(section, key)`.
- Uses `Newtonsoft.Json` for modern JSON parsing.
- Supports fallback for missing keys.
- Simple integration with UI and game state.

---

### Setup

1. Place language files (e.g., `en-US.json`, `es-ES.json`) in `Assets/StreamingAssets/Localization/`.
2. Ensure each file matches this structure:

```json
{
  "MainMenu": {
    "title": "Welcome",
    "start": "Start Game"
  },
  "OptionsMenu": {
    "language": "Language",
    "save": "Save"
  }
}
```

3. Call `LocalizationManager.LoadLanguage("en-US")` to initialize.

---

### Usage

#### Initialization

```csharp
LocalizationManager localizationManager = new LocalizationManager();
localizationManager.LoadLanguage("en-US");
```

#### Lookup

```csharp
string startLabel = localizationManager.Translate("MainMenu", "start");
// Returns: "Start Game"
```

#### Dynamic UI Example

```csharp
titleText.text = localizationManager.Translate("MainMenu", "title");
saveButtonText.text = localizationManager.Translate("OptionsMenu", "save");
```

### Notes

- Missing keys return `"[?Section.Key]"` for debugging.

---


#### InteractableObjects

A modular system for interacting with 3D objects in first-person games (e.g., doors, items, inspectables).
Includes raycast-based interaction, interfaces, and a plug-and-play item inspection system similar to those in horror games like Resident Evil or Visage.

#### Features

- Raycast-based interaction using player camera.
- Clean IInteractable interface for pluggable object behaviors.
- Reusable InspectableItem component for in-depth 3D item inspection.
- Fully controllable cursor/lock handling during inspection.
- Optional prefab override + custom initial rotation.
- Scroll-to-zoom + free rotation during inspection.

### Usage

1. Add the FPS Interaction Script
Create a script called FPSInteractor.cs and attach it to your FPS controller or camera:

```csharp
public class FPSInteractor : MonoBehaviour
{
    public float interactRange = 3f;
    public KeyCode interactKey = KeyCode.E;
    public LayerMask interactLayer;
    private Camera cam;

    void Start() => cam = Camera.main;

    void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            Ray ray = new Ray(cam.transform.position, cam.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactLayer))
            {
                var interactable = hit.collider.GetComponent<IInteractable>();
                interactable?.Interact();
            }
        }
    }
}
```

2. Add the Interactable Interface to the project

3. Create an Inspectable Item

Attach InspectableItem to any object you want to inspect.
NOTE: The item must belong to a layer for interaction that we must create.

---

