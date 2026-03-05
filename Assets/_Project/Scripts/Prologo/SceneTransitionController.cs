using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransitionController : MonoBehaviour
{

    [Header("Transición")]
    public GameObject transition;       // Panel negro que hace la cortina
    public float transitionTime = 1f;   // Tiempo de fade
    public float delayBeforeNext = 3f;  // Tiempo que se muestra el texto antes de pasar

    [Header("Escena siguiente")]
    public string siguienteEscena = "Orilla";

    void Start()
    {
        // Lanza la transición automáticamente
        StartCoroutine(TransicionAutomatica());
    }

    IEnumerator TransicionAutomatica()
    {
        // Espera el tiempo para que se lea el texto
        yield return new WaitForSeconds(delayBeforeNext);

        // Activa el panel negro
        transition.SetActive(true);

        // Aseguramos que tenga CanvasGroup
        CanvasGroup canvasGroup = transition.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = transition.AddComponent<CanvasGroup>();

        // FADE IN
        float elapsed = 0f;
        canvasGroup.alpha = 0f;

        while (elapsed < transitionTime)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsed / transitionTime);
            yield return null;
        }

        canvasGroup.alpha = 1f;

        // Cargamos la siguiente escena
        SceneManager.LoadScene(siguienteEscena);
    }
}