using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
// using FMODUnity; 

public class CinematicIntermedio2 : MonoBehaviour
{
    [Header("Actores y Cámaras")]
    public Transform actorCarrillo;
    public Animator animCarrillo;
    public Camera mainCamera;

    [Header("Escena Uchu (Imágenes)")]
    public GameObject fondoUchu; 
    public GameObject[] dialogosUchu; 
    public float tiempoPorDialogo = 3f; 

    [Header("Escena Moto (Sicario)")]
    public Transform imagenMoto; 
    public Transform destinoMoto; 
    public Transform encuadreMoto; 

    [Header("Marcadores de Movimiento (Carrillo)")]
    public Transform puntoEntrada;
    public Transform puntoBaseMuro; 
    public Transform puntoArribaMuro; 
    public Transform puntoSalida; 

    [Header("Configuraciones")]
    public float velocidadCaminar = 3f;
    public float velocidadTrepar = 2f;
    public float velocidadMoto = 15f;

    [Header("Transiciones y Escena")]
    public GameObject transitionPanel;
    public float transitionTime = 1f;
    public string siguienteEscena = "Intermedio3"; 

    private Vector3 posicionCamaraOriginal;

    void Start()
    {
        posicionCamaraOriginal = mainCamera.transform.position;
        
        
        if (fondoUchu != null) fondoUchu.SetActive(false);
        if (imagenMoto != null) imagenMoto.gameObject.SetActive(false);
        foreach (var dialogo in dialogosUchu) { dialogo.SetActive(false); }
        
        StartCoroutine(SecuenciaIntermedio2());
    }

    IEnumerator SecuenciaIntermedio2()
    {
        
        // [AUDIO: Iniciar pasos de Carrillo]
        actorCarrillo.position = puntoEntrada.position;
        yield return MoverActor(actorCarrillo, animCarrillo, puntoBaseMuro.position, "Correr", velocidadCaminar);
        // [AUDIO: Detener pasos]

        
        animCarrillo.Play("Trepar");
        // [AUDIO: Sonido de esfuerzo / trepar]
        yield return SubirMuro(actorCarrillo, puntoArribaMuro.position, velocidadTrepar);
        
        
        animCarrillo.Play("Agacharse"); // O el nombre que tenga tu animación
        // [AUDIO: Sonido de tela o movimiento sutil al agacharse]
        yield return new WaitForSeconds(1.5f);

        // FUNDIDO A NEGRO
        yield return HacerFade(1f);

        
        // [AUDIO: Cambio de música, ambiente tenso de Uchu]
        fondoUchu.SetActive(true);
        yield return HacerFade(0f); 

        
        foreach (GameObject dialogo in dialogosUchu)
        {
            dialogo.SetActive(true);
            // [AUDIO: Sonido de "blip" o impacto al aparecer el texto]
            yield return new WaitForSeconds(tiempoPorDialogo);
            dialogo.SetActive(false);
        }

        // FUNDIDO A NEGRO PARA SALIR DE UCHU
        yield return HacerFade(1f);
        fondoUchu.SetActive(false);
        // [AUDIO: Regresa el ambiente de la calle/noche]

        
        yield return HacerFade(0f); 
        yield return new WaitForSeconds(1f);

        
        yield return MoverCamara(encuadreMoto.position, 0.3f); 
        
        // [AUDIO: Arranca el motor de la moto a todo volumen]
        imagenMoto.gameObject.SetActive(true);
        yield return MoverVehiculo(imagenMoto, destinoMoto.position, velocidadMoto);
        imagenMoto.gameObject.SetActive(false); 

        
        yield return MoverCamara(posicionCamaraOriginal, 0.3f);
        
        
        // [AUDIO: Pasos corriendo rápido]
        yield return MoverActor(actorCarrillo, animCarrillo, puntoSalida.position, "Correr", velocidadCaminar * 1.5f); // Corre un poco más rápido
        // [AUDIO: Detener pasos]

        // 8. TRANSICIÓN FINAL AL SIGUIENTE NIVEL
        yield return HacerFade(1f);
        SceneManager.LoadScene(siguienteEscena);
    }

    // --- FUNCIONES HELPERS ---

    IEnumerator MoverActor(Transform actor, Animator anim, Vector3 destino, string animName, float vel)
    {
        anim.Play(animName); 
        while (Vector2.Distance(actor.position, destino) > 0.1f)
        {
            actor.position = Vector2.MoveTowards(actor.position, destino, vel * Time.deltaTime);
            yield return null; 
        }
        anim.Play("Idle");
    }

    IEnumerator SubirMuro(Transform actor, Vector3 destinoArriba, float vel)
    {
        // Movimiento puramente vertical/diagonal simulando escalar
        while (Vector2.Distance(actor.position, destinoArriba) > 0.1f)
        {
            actor.position = Vector2.MoveTowards(actor.position, destinoArriba, vel * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator MoverVehiculo(Transform vehiculo, Vector3 destino, float velocidad)
    {
        while (Vector2.Distance(vehiculo.position, destino) > 0.1f)
        {
            vehiculo.position = Vector2.MoveTowards(vehiculo.position, destino, velocidad * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator MoverCamara(Vector3 destino, float duracionViaje)
    {
        Vector3 posInicial = mainCamera.transform.position;
        Vector3 posFinal = new Vector3(destino.x, destino.y, posInicial.z); 
        float tiempo = 0;

        while (tiempo < duracionViaje)
        {
            mainCamera.transform.position = Vector3.Lerp(posInicial, posFinal, tiempo / duracionViaje);
            tiempo += Time.deltaTime;
            yield return null;
        }
        mainCamera.transform.position = posFinal;
    }

    // NUEVO: Función modular para prender/apagar la pantalla negra fácilmente
    IEnumerator HacerFade(float targetAlpha)
    {
        if (transitionPanel == null) yield break;

        transitionPanel.SetActive(true);
        CanvasGroup canvasGroup = transitionPanel.GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = transitionPanel.AddComponent<CanvasGroup>();

        float startAlpha = canvasGroup.alpha;
        float elapsed = 0f;

        while (elapsed < transitionTime)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / transitionTime);
            yield return null;
        }
        canvasGroup.alpha = targetAlpha;
        
        // Si se volvió transparente por completo, podemos apagar el panel para que no estorbe
        if (targetAlpha == 0f) transitionPanel.SetActive(false);
    }
}