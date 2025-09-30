using UnityEngine;

public class ItemInspector : Singleton<ItemInspector>
{
    public Transform inspectionPoint;
    public float rotationSpeed = 5f;

    public float zoomSpeed = 0.5f;
    public float minZoom = 0.5f;
    public float maxZoom = 2f;

    float currentZoom = 1f;
    Vector3 basePosition;

    GameObject currentItem;
    bool isInspecting = false;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Rigidbody originalRigidbody;
    private Collider originalCollider;

    void Update()
    {
        if (isInspecting && currentItem)
        {
            // Mouse rotation
            float rotX = Input.GetAxis("Mouse X") * rotationSpeed;
            float rotY = Input.GetAxis("Mouse Y") * rotationSpeed;

            currentItem.transform.Rotate(Vector3.up, -rotX, Space.World);
            currentItem.transform.Rotate(Vector3.right, rotY, Space.World);

            // Mouse scroll zoom
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (Mathf.Abs(scroll) > 0.01f)
            {
                currentZoom = Mathf.Clamp(currentZoom - scroll * zoomSpeed, minZoom, maxZoom);
                currentItem.transform.position = basePosition + inspectionPoint.forward * (currentZoom - 1f);
            }

            // Exit inspection on right-click
            if (Input.GetMouseButtonDown(1))
            {
                EndInspection();
            }
        }
    }

    public void Inspect(InspectableItem item)
    {
        if (isInspecting) return;

        isInspecting = true;
        currentItem = item.gameObject;

        // Store original transform
        originalPosition = currentItem.transform.position;
        originalRotation = currentItem.transform.rotation;

        // Move to inspection point
        currentItem.transform.position = inspectionPoint.position;

        // Store and disable Rigidbody
        originalRigidbody = currentItem.GetComponent<Rigidbody>();
        if (originalRigidbody != null)
        {
            originalRigidbody.isKinematic = true;
        }

        // Store and disable Collider
        originalCollider = currentItem.GetComponent<Collider>();
        if (originalCollider != null)
        {
            originalCollider.enabled = false;
        }

        basePosition = inspectionPoint.position;
        currentZoom = 1f;

        // Unlock cursor for inspection
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void EndInspection()
    {
        if (!isInspecting || currentItem == null) return;

        // Return item to original place
        currentItem.transform.position = originalPosition;
        currentItem.transform.rotation = originalRotation;

        // Re-enable Rigidbody
        if (originalRigidbody != null)
        {
            originalRigidbody.isKinematic = false;
        }

        // Re-enable Collider
        if (originalCollider != null)
        {
            originalCollider.enabled = true;
        }

        currentItem = null;
        isInspecting = false;

        // Re-lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
