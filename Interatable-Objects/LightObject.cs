using UnityEngine;

public class LightObject : MonoBehaviour, IInteractable
{
    private Light lightComponent;

    public void Start()
    {
        lightComponent = this.GetComponentInChildren<Light>();
        if (lightComponent == null)
        {
            Debug.LogError("No Light component found on this GameObject.");
        }
    }
    public void Interact()
    {
        Debug.Log("Interacted with light!");
        if (lightComponent != null)
        {
            lightComponent.enabled = !lightComponent.enabled;
        }
    }
}