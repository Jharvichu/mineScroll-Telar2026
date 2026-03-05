using UnityEngine;
using UnityEngine.SceneManagement;

public class RifleGuardController : MonoBehaviour
{
    
    public enum State { Patrol, SuspicionWait, Investigate, Aiming, Search, Return }
    public State currentState = State.Patrol;
    private Animator animator;

    [Header("Referencias")]
    public Transform pointA;
    public Transform pointB;
    private Transform currentPatrolTarget;

    [Header("Configuración de Velocidad")]
    public float patrolSpeed = 2f;
    public float suspiciousSpeed = 3f;

    [Header("Configuración de Visión (Francotirador)")]
    public float farVisionDistance = 12f;  // Rango largo para sospechar (?)
    public float nearVisionDistance = 8f;  // Rango para disparar (!)
    public LayerMask playerLayer;

    [Header("Temporizadores")]
    public float suspicionTime = 0.5f; 
    public float aimTime = 0.2f;    
    public float searchTime = 3f;    
    private float timer = 0f;

    private int facingDirection = 1;
    private Player.PlayerController targetPlayer; // angie 
    private Vector2 lastKnownPosition;
    

    void Start()
    {
        currentPatrolTarget = pointB;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Patrol:
                MoveTowards(currentPatrolTarget.position, patrolSpeed);
                CheckPatrolPoints();
                DetectPlayer();
                break;

            case State.SuspicionWait:
                animator.SetBool("isWalking", false);
                timer += Time.deltaTime;
                if (timer >= suspicionTime) ChangeState(State.Investigate);
                DetectPlayer(); 
                break;

            case State.Investigate:
                MoveTowards(lastKnownPosition, suspiciousSpeed);
                
                if (Mathf.Abs(transform.position.x - lastKnownPosition.x) < 0.5f)
                {
                    ChangeState(State.Search);
                }
                DetectPlayer(); 
                break;

            case State.Aiming:
                timer += Time.deltaTime;
                if (timer >= aimTime)
                {
                    if (targetPlayer != null && !targetPlayer.isHidden)
                    {
                        Debug.Log("¡BANG! Un solo tiro. GAME OVER.");
                        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    }
                    else
                    {
                        Debug.Log("¡Lo perdí de vista! Investigando...");
                        ChangeState(State.Search);
                    }
                }
                break;

            case State.Search:
                MoveTowards(lastKnownPosition, patrolSpeed);
                
                if (Mathf.Abs(transform.position.x - lastKnownPosition.x) < 0.5f)
                {
                    timer += Time.deltaTime;
                    if (timer >= searchTime) ChangeState(State.Return); 
                }
                DetectPlayer(); 
                break;

            case State.Return:
                MoveTowards(currentPatrolTarget.position, patrolSpeed);
                
                if (Mathf.Abs(transform.position.x - currentPatrolTarget.position.x) < 0.5f)
                {
                    ChangeState(State.Patrol); 
                }
                DetectPlayer();
                break;
        }
    }

    void DetectPlayer()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * facingDirection, farVisionDistance, playerLayer);
        Debug.DrawRay(transform.position, Vector2.right * facingDirection * farVisionDistance, Color.red);

        if (hit.collider != null)
        {
            Player.PlayerController player =    hit.collider.GetComponent<Player.PlayerController>(); // angie 

            if (player != null && !player.isHidden)
            {
                targetPlayer = player;
                float distance = Vector2.Distance(transform.position, player.transform.position);

                
                if (currentState != State.Aiming)
                {
                    
                    if (distance <= nearVisionDistance)
                    {
                        Debug.Log("¡TE VEO! Apuntando...");
                        lastKnownPosition = player.transform.position;
                        ChangeState(State.Aiming); 
                    }
                    
                    else if (distance <= farVisionDistance && currentState != State.Investigate && currentState != State.SuspicionWait)
                    {
                        Debug.Log("¿SOSPECHA (?)");
                        lastKnownPosition = player.transform.position;
                        ChangeState(State.SuspicionWait);
                    }
                }
            }
        }
    }

    void MoveTowards(Vector2 target, float speed)
    {
        transform.position = Vector2.MoveTowards(
        transform.position,
        new Vector2(target.x, transform.position.y),
        speed * Time.deltaTime
    );

        animator.SetBool("isWalking", true);

        if (target.x > transform.position.x + 0.1f) Flip(1);
        else if (target.x < transform.position.x - 0.1f) Flip(-1);
    }

    void CheckPatrolPoints()
    {
        
        if (Mathf.Abs(transform.position.x - currentPatrolTarget.position.x) < 0.5f)
        {
            currentPatrolTarget = (currentPatrolTarget == pointA) ? pointB : pointA;
        }
    }

    void ChangeState(State newState)
    {
        currentState = newState;
        timer = 0f;

        if (newState == State.Aiming)
        {
            animator.SetBool("isWalking", false); // deja de caminar
            animator.SetTrigger("Dispara");       // activa animación disparo
        }

        if (newState == State.Patrol ||
            newState == State.Investigate ||
            newState == State.Search ||
            newState == State.Return)
        {
            animator.SetBool("isWalking", true); // vuelve a caminar
        }
    }

    void Flip(int direction)
    {
        if (facingDirection == direction) return; 
        facingDirection = direction;
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * direction;
        transform.localScale = scale;
    }
}