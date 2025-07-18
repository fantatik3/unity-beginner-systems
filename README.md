# Unity Helpers

This repository contains a collection of commonly used helper classes for the Unity Engine.


## Index

- [AssetBundle](#assetbundle)
- [SceneLoader](#sceneloader)

---

### AssetBundle
A basic class to build AssetBundles and create a JSON hash list for later download.
[AssetBundles - Unity Manual](https://docs.unity3d.com/Manual/AssetBundlesIntro.html)

> **Note:** Unity now provides [Addressables](https://docs.unity3d.com/Packages/com.unity.addressables@latest) as a high-level API to manage remote content. It is recommended to use Addressables instead of handling AssetBundles manually in most cases.

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
2. (Optional) Assign a scene name to `loadingSceneName` if using a loading screen.
3. Mark the GameObject as `DontDestroyOnLoad`.

#### Load a Scene with optional loading screen

```csharp
// Load "MainMenu" scene with a loading screen
SceneLoader.Instance.LoadScene("MainMenu", useLoadingScene: true);

// Load "Level1" scene directly
SceneLoader.Instance.LoadScene("Level1");
```
---
