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
