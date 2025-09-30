using UnityEngine;

public class DoorObject : MonoBehaviour, IInteractable
{
    public Transform hinge;
    public float openAngle = 90f;
    public float openSpeed = 2f;

    private bool isOpen = false;
    private float currentAngle = 0f;
    private float targetAngle = 0f;


    public void Start()
    {
        if (hinge == null)
        {
            Debug.LogError("Hinge Transform is not assigned.");
        }
    }

    public void Update()
    {
        if (isOpen)
        {
            currentAngle = Mathf.Lerp(currentAngle, targetAngle, Time.deltaTime * openSpeed);
            hinge.localRotation = Quaternion.Euler(0f, currentAngle, 0f);
        }
        else
        {
            currentAngle = Mathf.Lerp(currentAngle, 0f, Time.deltaTime * openSpeed);
            hinge.localRotation = Quaternion.Euler(0f, currentAngle, 0f);
        }
    }



    public void Interact()
    {
        Debug.Log("Interacted with door!");
        isOpen = !isOpen;
        targetAngle = isOpen ? openAngle : 0f;
    }
}