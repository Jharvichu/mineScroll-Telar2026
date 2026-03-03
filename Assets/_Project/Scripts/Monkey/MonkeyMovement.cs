using UnityEngine;

public class MonkeyMovement : MonoBehaviour
{
    [Header("Puntos")]
    public Transform startPoint;
    public Transform endPoint;

    [Header("Configuración")]
    public float speed = 3f;
    public float idleTime = 2f; 

    private Animator animator;
    private float timer;
    private bool isMoving = false;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (startPoint != null)
            transform.position = startPoint.position;

        animator.SetBool("isRunning", false); 
    }

    void Update()
    {
        if (!isMoving)
        {
            timer += Time.deltaTime;

            if (timer >= idleTime)
            {
                isMoving = true;
                animator.SetBool("isRunning", true);
            }

            return;
        }

        MoveToEnd();
    }

    void MoveToEnd()
    {
        if (endPoint == null) return;

        
        if (endPoint.position.x > transform.position.x)
            transform.localScale = new Vector3(1, 1, 1);
        else
            transform.localScale = new Vector3(-1, 1, 1);

        transform.position = Vector2.MoveTowards(
            transform.position,
            endPoint.position,
            speed * Time.deltaTime
        );

        if (Vector2.Distance(transform.position, endPoint.position) < 0.1f)
        {
            Destroy(gameObject);
        }
    }
}
