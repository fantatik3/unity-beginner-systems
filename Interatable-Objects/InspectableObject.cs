using UnityEngine;

public class InspectableObject : InteractableBase, IInteractable
{
    public GameObject inspectionPrefab;
    public override void Interact()
    {
        Debug.Log("Interacted with inspectable item!");
        if (inspectionPrefab != null)
        {
            ItemInspector.Instance.Inspect(this);
        }
    }
}
