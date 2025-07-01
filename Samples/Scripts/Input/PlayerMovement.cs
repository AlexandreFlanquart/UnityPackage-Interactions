using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Références")]
    [SerializeField] private Transform cameraHolder; // Assigné dans l'inspecteur (pivot vertical)
    [SerializeField] private Rigidbody rb;

    [Header("Paramètres")]
    [SerializeField] private float lookSensitivity = 0.4f;
    [SerializeField] private float maxPitch = 89f;
    [SerializeField] private float moveSpeed = 20f;

    private float pitch = 0f;
    private float yaw = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        // Verrouille le curseur pour l'expérience FPS
        Cursor.lockState = CursorLockMode.Locked;

        rb = GetComponent<Rigidbody>();
        if (rb == null) rb = GetComponent<Rigidbody>();
        yaw = transform.eulerAngles.y;

        // Abonnement aux événements InputManager
        InputManager.GetInstance().OnPressDirection += Move;
        InputManager.GetInstance().OnLookDirection += LookAround;
    }

    // Déplacement du joueur (appelé par InputManager)
    private void Move(Vector2 move)
    {
        Vector3 direction = new Vector3(move.x, 0, move.y);
        direction = transform.TransformDirection(direction);
        rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
    }

    // Rotation FPS (appelée par InputManager)
    private void LookAround(Vector2 look)
    {
        yaw += look.x * lookSensitivity;
        pitch -= look.y * lookSensitivity;
        pitch = Mathf.Clamp(pitch, -maxPitch, maxPitch);

        // Rotation horizontale du joueur (yaw)
        transform.rotation = Quaternion.Euler(0, yaw, 0);
        // Rotation verticale de la caméra (pitch)
        if (cameraHolder != null)
            cameraHolder.localRotation = Quaternion.Euler(pitch, 0, 0);
    }
}

