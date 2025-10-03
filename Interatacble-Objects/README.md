

### InteractableObjects

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
