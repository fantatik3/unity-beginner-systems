using UnityEngine;

public class DoorObject : InteractableBase, IInteractable
{
    public Transform hinge;
    public float openAngle = 90f;
    public float openSpeed = 2f;
    public bool openToLeft = false; // false = right (positive angle), true = left (negative angle)

    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;
    private Quaternion targetRotation;
    private bool isRotating = false;

    private void Start()
    {
        if (hinge == null)
        {
            Debug.LogError("Hinge Transform is not assigned.", this);
            enabled = false;
            return;
        }

        float angle = openToLeft ? -openAngle : openAngle;

        closedRotation = hinge.rotation;
        openRotation = Quaternion.AngleAxis(angle, Vector3.up) * closedRotation;
        targetRotation = closedRotation;
    }

    private void Update()
    {
        if (!isRotating) return;

        hinge.rotation = Quaternion.RotateTowards(
            hinge.rotation,
            targetRotation,
            openSpeed * Time.deltaTime * 100f
        );

        if (Quaternion.Angle(hinge.localRotation, targetRotation) < 0.1f)
        {
            hinge.localRotation = targetRotation;
            isRotating = false;
        }
    }

    public override void Interact()
    {
        isOpen = !isOpen;
        targetRotation = isOpen ? openRotation : closedRotation;
        isRotating = true;
        Debug.Log($"Door {(isOpen ? "opened" : "closed")}.", this);
    }
}
