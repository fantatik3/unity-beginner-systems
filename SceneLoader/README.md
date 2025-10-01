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
