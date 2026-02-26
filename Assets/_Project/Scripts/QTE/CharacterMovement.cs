using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float runSpeed = 5f; 
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    public bool canMove = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (!canMove)
        {
            // Detener completamente al jugador mientras no puede moverse
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return; // salir del Update
        }

        // Movimiento normal
        if (Input.GetKey("d") || Input.GetKey(KeyCode.RightArrow))
        {
            rb.linearVelocity = new Vector2(runSpeed, rb.linearVelocity.y);
            spriteRenderer.flipX = false;
        }
        else if (Input.GetKey("a") || Input.GetKey(KeyCode.LeftArrow))
        {
            rb.linearVelocity = new Vector2(-runSpeed, rb.linearVelocity.y);
            spriteRenderer.flipX = true;
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
    }
}
