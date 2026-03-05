using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; 

public class EnemyController : MonoBehaviour
{
    public enum State { Patrol, SuspicionWait, Investigate, AlertWait, Chase, Search, Return, Catching }
    public State currentState = State.Patrol;

    [Header("Referencias")]
    public Transform pointA;
    public Transform pointB;
    private Transform currentPatrolTarget;

    [Header("Configuración de Velocidad")]
    public float patrolSpeed = 2f;
    public float suspiciousSpeed = 3f; 
    public float chaseSpeed = 6f;    

    [Header("Configuración de Visión")]
    public float farVisionDistance = 5f;   
    public float nearVisionDistance = 8f;  
    public LayerMask playerLayer;
    public float catchDistance = 2.0f; 
    
    
    [Tooltip("Ajusta la altura del láser. Valores negativos lo bajan a la cintura/piernas.")]
    public float visionHeightOffset = -0.5f;

    [Header("Temporizadores (MGS)")]
    public float reactionTime = 0.3f; 
    public float searchTime = 3f;   
    private float timer = 0f;

    private int facingDirection = 1;
    private Player.PlayerController targetPlayer; 
    private Vector2 lastKnownPosition;
    private Animator animator;
    
    private bool isCatching = false; 

    void Start()
    {
        currentPatrolTarget = pointB;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        
        if (isCatching) return;

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
                if (timer >= reactionTime) ChangeState(State.Investigate);
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

            case State.AlertWait:
                timer += Time.deltaTime;
                if (timer >= reactionTime) ChangeState(State.Chase);
                break;

            case State.Chase:
                if (targetPlayer != null)
                {
                    MoveTowards(targetPlayer.transform.position, chaseSpeed);
                    lastKnownPosition = targetPlayer.transform.position;

                    
                    if (Vector2.Distance(transform.position, targetPlayer.transform.position) <= catchDistance)
                    {
                        StartCoroutine(AtraparJugador()); 
                    }

                    
                    if (targetPlayer.isHidden)
                    {
                        Debug.Log("¡Se escondió! Buscando...");
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
        
        Vector2 originPoint = new Vector2(transform.position.x, transform.position.y + visionHeightOffset);

        
        RaycastHit2D hit = Physics2D.Raycast(originPoint, Vector2.right * facingDirection, farVisionDistance, playerLayer);
        
        
        Debug.DrawRay(originPoint, Vector2.right * facingDirection * farVisionDistance, Color.red);

        if (hit.collider != null)
        {
            Player.PlayerController player = hit.collider.GetComponent<Player.PlayerController>(); 
            

            if (player != null && !player.isHidden)
            {
                targetPlayer = player;
                float distance = Vector2.Distance(transform.position, player.transform.position);

                if (currentState != State.Chase && currentState != State.AlertWait && currentState != State.Catching)
                {
                    if (distance <= nearVisionDistance)
                    {
                        Debug.Log("¡ALERTA (!)");
                        lastKnownPosition = player.transform.position;
                        ChangeState(State.AlertWait);
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
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.x, transform.position.y), speed * Time.deltaTime);
        animator.SetBool("isWalking", true);

        if (target.x > transform.position.x + 0.1f) Flip(1);
        else if (target.x < transform.position.x - 0.1f) Flip(-1);
    }

    void CheckPatrolPoints()
    {
        if (Mathf.Abs(transform.position.x - currentPatrolTarget.position.x) < 0.5f)
        {
            animator.SetBool("isWalking", false);
            currentPatrolTarget = (currentPatrolTarget == pointA) ? pointB : pointA;
        }
    }

    void ChangeState(State newState)
    {
        currentState = newState;
        timer = 0f;
    }

    void Flip(int direction)
    {
        if (facingDirection == direction) return; 
        facingDirection = direction;
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * direction;
        transform.localScale = scale;
    }

    
    private IEnumerator AtraparJugador()
    {
        isCatching = true;
        currentState = State.Catching; 

        
        animator.SetBool("isWalking", false); 
        animator.SetBool("Ataca", true);

        
        targetPlayer.canControl = false;
        targetPlayer.Rigidbody2D.linearVelocity = Vector2.zero;

        Debug.Log("¡GAME OVER! El guardia te está atacando...");

        
        yield return new WaitForSeconds(0.5f);

       
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}