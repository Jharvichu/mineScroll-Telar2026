using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class MonkeyMove : MonoBehaviour
{
    public float velocidad = 3f;

    private Transform destino;
    private bool mover = false;
    private bool destruirAlLlegar = false;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        EventInstance bgm = AudioManager.Instance.GetBGMInstance();
        RuntimeManager.AttachInstanceToGameObject(bgm, gameObject);
    }

    void Update()
    {
        if (mover && destino != null)
        {
            animator.SetBool("isRunning", true);

            transform.position = Vector2.MoveTowards(
                transform.position,
                destino.position,
                velocidad * Time.deltaTime
            );

            if (Vector2.Distance(transform.position, destino.position) < 0.1f)
            {
                mover = false;
                animator.SetBool("isRunning", false);

                if (destruirAlLlegar) {
                    Destroy(gameObject); 
                }
            }
        }
    }

    public void IrA(Transform punto, bool destruir = false)
    {
        destino = punto;
        mover = true;
        destruirAlLlegar = destruir;
        RuntimeManager.PlayOneShotAttached("event:/SFX_MONO", gameObject);
    }
}