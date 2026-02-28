using UnityEngine;

public class DummyPlayerTest : MonoBehaviour
{
    public float speed = 5f;
    public bool isHidden = false;
    private Rigidbody2D rb;

    void Start() { rb = GetComponent<Rigidbody2D>(); }

    void Update()
    {

        float moveX = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(moveX * speed, rb.linearVelocity.y);

       
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isHidden = !isHidden;
            GetComponent<SpriteRenderer>().color = isHidden ? new Color(1, 1, 1, 0.3f) : Color.blue; // Se hace semi-transparente
            Debug.Log(isHidden ? "Me escondí" : "Salí del escondite");
        }
    }
}