using UnityEngine;

public class InspectableItem : MonoBehaviour, IInteractable
{
    public GameObject inspectionPrefab;

    public void Interact()
    {
        Debug.Log("Interacted with inspectable item!");
        if (inspectionPrefab != null)
        {
            ItemInspector.Instance.Inspect(this);
        }
    }
}
