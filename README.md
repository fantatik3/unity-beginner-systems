
# Unity Helpers

This repository contains a collection of commonly used helper classes for the Unity Engine.


## Index

- [AssetBundle](#assetbundle)
- [Singleton\<T\>](#singletont)
- [SceneLoader](#sceneloader)

---

### AssetBundle
A basic class to build AssetBundles and create a JSON hash list for later download.
[AssetBundles - Unity Manual](https://docs.unity3d.com/Manual/AssetBundlesIntro.html)

> **Note:** Unity now provides [Addressables](https://docs.unity3d.com/Packages/com.unity.addressables@latest) as a high-level API to manage remote content. It is recommended to use Addressables instead of handling AssetBundles manually in most cases.

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
