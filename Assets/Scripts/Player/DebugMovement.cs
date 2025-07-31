using UnityEngine;

public class DebugMovement : MonoBehaviour
{
    [Header("Toggle")]
    public KeyCode toggleKey = KeyCode.F2;
    public bool startEnabled = false;

    [Header("Speed")]
    public float speed = 10f;            // current fly speed (scroll wheel adjusts)
    public float minSpeed = 1f;
    public float maxSpeed = 100f;
    public float scrollSensitivity = 5f; // how much to change per scroll tick

    [Header("Speed Modifiers (while flying)")]
    public float fastMultiplier = 3f;    // hold Shift
    public float slowMultiplier = 0.25f; // hold Ctrl

    [Header("Vertical Keys")]
    public KeyCode upKey = KeyCode.Space;
    public KeyCode downKey = KeyCode.LeftControl;

    [Header("Mouse Look")]
    public Transform lookRoot;        // assign your Camera; if null, auto-detect
    public float lookSensitivity = 2f;
    public bool invertY = false;

    // References to disable
    private FirstPersonController controller;
    private Rigidbody rb;
    private CapsuleCollider collider;

    private bool debugMode = false;
    private float yaw;
    private float pitch;

    void Start()
    {
        controller = GetComponent<FirstPersonController>();
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<CapsuleCollider>();

        // Auto-assign lookRoot if not set
        if (lookRoot == null && Camera.main != null)
            lookRoot = Camera.main.transform;

        // Initialize yaw/pitch from current rotation
        yaw = transform.eulerAngles.y;
        if (lookRoot != null)
            pitch = NormalizeAngle(lookRoot.localEulerAngles.x);
        else
            pitch = NormalizeAngle(transform.eulerAngles.x);

        if (startEnabled)
            EnableDebugMode();
    }

    void Update()
    {
        // Toggle
        if (Input.GetKeyDown(toggleKey))
        {
            if (debugMode) DisableDebugMode();
            else EnableDebugMode();
        }

        if (!debugMode) return;

        // Scroll speed
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            speed = Mathf.Clamp(speed + scroll * scrollSensitivity, minSpeed, maxSpeed);
        }

        HandleLook();
        HandleMove();
    }

    private void HandleLook()
    {
        float mx = Input.GetAxisRaw("Mouse X") * lookSensitivity;
        float my = Input.GetAxisRaw("Mouse Y") * lookSensitivity * (invertY ? 1f : -1f);

        yaw += mx;
        pitch += my;
        pitch = Mathf.Clamp(pitch, -89f, 89f);

        if (lookRoot != null && lookRoot != transform)
        {
            // rotate body on yaw only
            transform.rotation = Quaternion.Euler(0f, yaw, 0f);
            // rotate camera locally for pitch
            lookRoot.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        }
        else
        {
            // no camera root: rotate whole player with pitch + yaw
            transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
        }
    }

    private void HandleMove()
    {
        float currentSpeed = speed;
        if (Input.GetKey(KeyCode.LeftShift)) currentSpeed *= fastMultiplier;
        if (Input.GetKey(KeyCode.LeftControl)) currentSpeed *= slowMultiplier;

        // Basis for forward/right should match body yaw, not pitch
        Transform basis = (lookRoot != null && lookRoot != transform) ? transform : (lookRoot ?? transform);

        float h = Input.GetAxisRaw("Horizontal"); // A/D
        float v = Input.GetAxisRaw("Vertical");   // W/S

        Vector3 move = (basis.forward * v) + (basis.right * h);

        // Vertical fly keys
        if (Input.GetKey(upKey)) move += Vector3.up;
        if (Input.GetKey(downKey)) move += Vector3.down;

        if (move.sqrMagnitude > 1f) move.Normalize();

        transform.position += move * currentSpeed * Time.unscaledDeltaTime;
    }

    private void EnableDebugMode()
    {
        debugMode = true;

        if (controller) controller.enabled = false;
        if (rb)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
            rb.linearVelocity = Vector3.zero;
        }
        if (collider) collider.enabled = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void DisableDebugMode()
    {
        debugMode = false;

        if (controller) controller.enabled = true;
        if (rb)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
        }
        if (collider) collider.enabled = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private float NormalizeAngle(float angle)
    {
        // convert 0..360 to -180..180
        if (angle > 180f) angle -= 360f;
        return angle;
    }
}
