using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Récupérer l'input des touches ZQSD (WASD par position physique)
        moveInput.x = 0f;
        moveInput.y = 0f;

        if (Input.GetKey(KeyCode.W))
            moveInput.y = 1f;  // Haut (Z sur AZERTY)
        if (Input.GetKey(KeyCode.S))
            moveInput.y = -1f; // Bas (S sur AZERTY)
        if (Input.GetKey(KeyCode.A))
            moveInput.x = -1f; // Gauche (Q sur AZERTY)
        if (Input.GetKey(KeyCode.D))
            moveInput.x = 1f;  // Droite (D sur AZERTY)

        // Normaliser pour éviter une diagonale plus rapide
        moveInput = moveInput.normalized;
    }

    void FixedUpdate()
    {
        // Appliquer le mouvement
        rb.linearVelocity = moveInput * moveSpeed;
    }
}
