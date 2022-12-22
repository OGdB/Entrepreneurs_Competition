using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    [SerializeField]
    private float PanSpeed = 20f;
    [SerializeField]
    private float ZoomSpeedTouch = 0.1f;
    [SerializeField]
    private float ZoomSpeedMouse = 0.5f;
    [SerializeField]
    private float[] BoundsX = new float[] { -300f, 300f };
    [SerializeField]
    private float[] BoundsZ = new float[] { -300f, 300f };
    [SerializeField]
    private float[] ZoomBounds = new float[] { 17.5f, 60f };

    private Camera cam;

    private Vector3 lastPanPosition;
    private int panFingerId; // Touch mode only

    private bool wasZoomingLastFrame; // Touch mode only
    private Vector2[] lastZoomPositions; // Touch mode only

    private void Awake() => cam = Camera.main;

    private void Update()
    {
        if (Input.touchSupported && Application.platform != RuntimePlatform.WebGLPlayer)
        {
            HandleTouch();
        }
        else
        {
            HandleMouse();
        }
    }

    private void HandleTouch()
    {
        switch (Input.touchCount)
        {

            case 1: // Panning
                wasZoomingLastFrame = false;

                // If the touch began, capture its position and its finger ID.
                // Otherwise, if the finger ID of the touch doesn't match, skip it.
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    lastPanPosition = touch.position;
                    panFingerId = touch.fingerId;
                }
                else if (touch.fingerId == panFingerId && touch.phase == TouchPhase.Moved)
                {
                    PanCamera(touch.position);
                }
                break;

            case 2: // Zooming
                Vector2[] newPositions = new Vector2[] { Input.GetTouch(0).position, Input.GetTouch(1).position };
                if (!wasZoomingLastFrame)
                {
                    lastZoomPositions = newPositions;
                    wasZoomingLastFrame = true;
                }
                else
                {
                    // Zoom based on the distance between the new positions compared to the 
                    // distance between the previous positions.
                    float newDistance = Vector2.Distance(newPositions[0], newPositions[1]);
                    float oldDistance = Vector2.Distance(lastZoomPositions[0], lastZoomPositions[1]);
                    float offset = newDistance - oldDistance;

                    ZoomCamera(offset, ZoomSpeedTouch);

                    lastZoomPositions = newPositions;
                }
                break;

            default:
                wasZoomingLastFrame = false;
                break;
        }
    }

    private void HandleMouse()
    {
        // On mouse down, capture it's position.
        // Otherwise, if the mouse is still down, pan the camera.
        if (Input.GetMouseButtonDown(0))
        {
            lastPanPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            PanCamera(Input.mousePosition);
        }

        // Check for scrolling to zoom the camera
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        ZoomCamera(scroll, ZoomSpeedMouse);
    }

    private void PanCamera(Vector3 newPanPosition)
    {
        Vector3 forward = transform.forward;
        forward.y = 0;

        // Determine how much to move the camera
        // Offset = amount of pixels the mouse moved right & up
        Vector3 offset = cam.ScreenToViewportPoint(lastPanPosition - newPanPosition);
        Vector3 move = ((transform.right * offset.x) + (forward * offset.y)) * PanSpeed;

        // Perform the movement
        transform.Translate(move, Space.World);

        // Ensure the camera remains within bounds.
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(transform.position.x, BoundsX[0], BoundsX[1]);
        pos.z = Mathf.Clamp(transform.position.z, BoundsZ[0], BoundsZ[1]);
        transform.position = pos;

        // Cache the position
        lastPanPosition = newPanPosition;
    }

    private void ZoomCamera(float offset, float speed)
    {
        if (offset == 0)
        {
            return;
        }

        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView - (offset * speed), ZoomBounds[0], ZoomBounds[1]);
    }

    /// <summary>
    /// Set the point in the parameter to the center of the screen (without changing the camera's rotation).
    /// </summary>
    /// <param name="point"></param>
    public static void CenterCameraOnPoint(Vector3 point)
    {
        Transform cameraTransform = Camera.main.transform;

        //get the mask to raycast against either the player or enemy layer
        int layer_mask = LayerMask.GetMask("Ground");

        //do the raycast specifying the mask
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit, Mathf.Infinity, layer_mask))
        {
            Vector3 cameraOffsetToCenter = cameraTransform.position - hit.point;
            cameraTransform.position = point + cameraOffsetToCenter;
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Vector3 forward = transform.forward;
        forward.y = 0;
        forward *= 20;
        Ray ray = new(transform.position, forward);
        Gizmos.DrawRay(r: ray);
    }
}