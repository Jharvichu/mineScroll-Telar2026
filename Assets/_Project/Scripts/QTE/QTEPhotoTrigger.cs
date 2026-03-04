using UnityEngine;
using UnityEngine.SceneManagement;
using Player;

public class QTEPhotoTrigger : MonoBehaviour
{
    [Header("Conexión UI")]
    public CircularQTE qteUI; 
[Header("Posición en Pantalla (X, Y)")]
    public Vector2 positionOnScreen = new Vector2(0, 0); 
    [Header("Configuración Única de este QTE")]
    public float needleSpeed = 200f;
    [Range(0, 360)] public float startAngle = 225f; 
    [Range(10, 180)] public float zoneSize = 90f;   

    [Header("Castigos")]
    [Tooltip("0 = Muerte súbita, -1 = Intentos infinitos")]
    public int allowedFails = 1; 
    private int currentFails = 0;

    private bool hasTriggered = false;
    private PlayerController trappedPlayer;

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (!hasTriggered && other.CompareTag("Player")) 
        {
            trappedPlayer = other.GetComponent<PlayerController>();
            if (trappedPlayer != null)
            {
                StartCinematicQTE();
            }
        }
    }

    void StartCinematicQTE()
    {
        hasTriggered = true;
        
        // 1. Congelar a Carrillo físicamente
        trappedPlayer.canControl = false;
        trappedPlayer.Rigidbody2D.linearVelocity = Vector2.zero;

        // (Animación desactivada hasta que la metas al Animator)
        // Animator anim = trappedPlayer.GetComponentInChildren<Animator>();
        // if (anim != null) anim.Play("FOto"); 

        // Encender el cerebro visual en la posición deseada
        qteUI.StartQTE(needleSpeed, startAngle, zoneSize, positionOnScreen, OnQTESuccess, OnQTEFail);
    }

    void OnQTESuccess()
    {
        // ¡Foto tomada! Le devolvemos el control a Carrillo
        trappedPlayer.canControl = true;
        
        // (Animación desactivada)
        // Animator anim = trappedPlayer.GetComponentInChildren<Animator>();
        // if (anim != null) anim.Play("Idle"); 
        
        Debug.Log("¡CLICK! Foto perfecta. QTE superado.");
        Destroy(gameObject); // Destruimos el cubo invisible para no repetirlo
    }

    void OnQTEFail()
    {
        if (allowedFails == -1)
        {
            Debug.Log("Fallaste. Intentos infinitos, repitiendo...");
           qteUI.StartQTE(needleSpeed, startAngle, zoneSize, positionOnScreen, OnQTESuccess, OnQTEFail);
        }
        else
        {
            currentFails++;
            if (currentFails > allowedFails)
            {
                Debug.Log("Demasiados fallos. GAME OVER.");
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            else
            {
                Debug.Log($"Fallaste. Te quedan {allowedFails - currentFails + 1} intentos.");
                qteUI.StartQTE(needleSpeed, startAngle, zoneSize, positionOnScreen, OnQTESuccess, OnQTEFail);
            }
        }
    }
}