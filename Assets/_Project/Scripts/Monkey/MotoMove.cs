using Player;
using UnityEngine;

public class MotoMove : MonoBehaviour
{

    public Transform pointA;
    public Transform pointB;
    public float speed = 8f;

    private bool isMoving = false;

    private SpriteRenderer sprite;
    private Collider2D col;
    private PlayerController playerController;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        sprite.enabled = false;   // invisible
        col.enabled = false;      // 🔥 no colisiona

        if (pointA != null)
            transform.position = pointA.position;
    }

    void Update()
    {
        if (!isMoving) return;

        transform.position = Vector2.MoveTowards(
            transform.position,
            pointB.position,
            speed * Time.deltaTime
        );

        if (Vector2.Distance(transform.position, pointB.position) < 0.05f)
        {
            if (playerController != null)
                playerController.canControl = true;
            playerController.Rigidbody2D.bodyType = RigidbodyType2D.Dynamic;

            Destroy(gameObject);
        }
    }

    public void ActivateMoto(PlayerController controller)
    {
        playerController = controller;

        sprite.enabled = true;
        col.enabled = true;   // 🔥 recién ahora colisiona
        isMoving = true;
    }
}