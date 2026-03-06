using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Player; 

public class LevelExitTrigger : MonoBehaviour
{
    [Header("Transición")]
    public GameObject transitionPanel;  
    public float transitionTime = 1f;   

    [Header("Escena Destino")]
    public string siguienteEscena = "Intermedio1"; 

    private bool yaActivado = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!yaActivado && collision.CompareTag("Character"))
        {
            yaActivado = true;
            Debug.Log("1. ¡Roldan tocó la puerta!");
            
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.canControl = false;
                player.Rigidbody2D.linearVelocity = Vector2.zero;
                Debug.Log("2. Roldan congelado exitosamente.");
                
            }

            Debug.Log("3. Arrancando corrutina de pantalla negra...");
            StartCoroutine(FadeOutYCambiarEscena());
        }
    }

    IEnumerator FadeOutYCambiarEscena()
    {
        if (transitionPanel == null) 
        {
            Debug.LogError("¡ERROR! No asignaste el panel negro en el Inspector.");
            yield break;
        }

        transitionPanel.SetActive(true);
        Debug.Log("4. Panel negro encendido.");

        CanvasGroup canvasGroup = transitionPanel.GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = transitionPanel.AddComponent<CanvasGroup>();

        float elapsed = 0f;
        canvasGroup.alpha = 0f;

        while (elapsed < transitionTime)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsed / transitionTime);
            yield return null;
        }

        canvasGroup.alpha = 1f;
        Debug.Log("5. Pantalla totalmente negra. Esperando medio segundo...");

        yield return new WaitForSeconds(0.5f);

        Debug.Log("6. ¡Viajando a " + siguienteEscena + "!");
        SceneManager.LoadScene(siguienteEscena);
    }
}