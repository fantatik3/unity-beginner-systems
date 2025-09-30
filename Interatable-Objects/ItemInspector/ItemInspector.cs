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

    protected override void Awake()
    {
        base.Awake();
    }

    void Update()
    {
        if (isInspecting && currentItem) { 
        
            float rotX = Input.GetAxis("Mouse X") * rotationSpeed;
            float rotY = Input.GetAxis("Mouse Y") * rotationSpeed;
            float scroll = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
            currentItem.transform.Rotate(Vector3.up, -rotX, Space.World);
            currentItem.transform.Rotate(Vector3.right, rotY, Space.World);

            if (Mathf.Abs(scroll) > 0.01f)
            {
                currentZoom = Mathf.Clamp(currentZoom - scroll, minZoom, maxZoom);
                currentItem.transform.position = basePosition + inspectionPoint.forward * (currentZoom - 1f);
            }

            //Right click to exit inspection
            if (Input.GetMouseButtonDown(1)) {
                EndInspection();
            }
        }
    }

    public void Inspect(InspectableItem item) {

        if (isInspecting) return;

        isInspecting = true;

        GameObject objToInspect = item.inspectionPrefab != null 
            ? Instantiate(item.inspectionPrefab)
            : Instantiate(item.gameObject);

        currentItem = objToInspect;

        objToInspect.transform.position = inspectionPoint.position;

        basePosition = inspectionPoint.position;

        currentZoom = 1f;

        Rigidbody rb = objToInspect.GetComponent<Rigidbody>();
        if (rb) { Destroy(rb); }

        Collider col = objToInspect.GetComponent<Collider>();
        if (col) { Destroy(col); }

        //Disable player input
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void EndInspection() { 
        if (currentItem) {
            Destroy(currentItem);
        }
        isInspecting = false;

        //Re-Enable player input
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

}
