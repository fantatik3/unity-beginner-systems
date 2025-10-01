using UnityEngine;

public abstract class InteractableBase : MonoBehaviour, IInteractable
{

    [SerializeField] private string displayName = "TEST NAME";
    [SerializeField] private eINTERACTABLE_TYPES type = eINTERACTABLE_TYPES.NONE;

    //Methods we want to override here
    public abstract void Interact();

    //Shared methods
    public virtual string GetName()
    {
        Debug.Log($"Object Name: {displayName}");
        return displayName;
    }

    public virtual eINTERACTABLE_TYPES GetInteractableType() {
        return type;
    }

}
