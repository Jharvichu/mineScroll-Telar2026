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

    [Header("Escena Uchu (Conversación)")]
    public GameObject fondoUchu; 
    [Tooltip("Pon los PNGs en el orden que hablan (Uchu, Sicario, Uchu...)")]
    public GameObject[] dialogosConversacion; 
    public float tiempoPorDialogo = 3f; 

    [Header("Interfaz y POV (Para la foto de la visión)")]
    public CircularQTE qteUI;
    public GameObject hudCamaraPOV; 
    public float tiempoLimiteQTE = 1.5f;

    [Header("Escena Moto (Sicario)")]
    public Transform imagenMoto; 
    public Transform destinoMoto; 
    public Transform encuadreMoto; 

    [Header("Marcadores de Movimiento (Carrillo)")]
    public Transform puntoEntrada;
    public Transform puntoBaseMuro; 
    public Transform puntoArribaMuro; 
    public Transform puntoAterrizaje; 
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
        if (hudCamaraPOV != null) hudCamaraPOV.SetActive(false);
        if (transitionPanel != null) transitionPanel.SetActive(false);

        foreach (var dialogo in dialogosConversacion) { dialogo.SetActive(false); }
        
        StartCoroutine(SecuenciaIntermedio2());
    }

    IEnumerator SecuenciaIntermedio2()
    {
        
        // [AUDIO: Pasos Carrillo INICIO]
        actorCarrillo.position = puntoEntrada.position;
        yield return MoverActor(actorCarrillo, animCarrillo, puntoBaseMuro.position, "Correr", velocidadCaminar);
        // [AUDIO: Pasos Carrillo FIN]

        
        animCarrillo.Play("Trepar");
        // [AUDIO: Esfuerzo / Trepar]
        yield return SubirMuro(actorCarrillo, puntoArribaMuro.position, velocidadTrepar);
        
        
        animCarrillo.Play("Agacharse"); 
        // [AUDIO: Ropa moviéndose al agacharse]
        yield return new WaitForSeconds(1.5f);

        
        yield return HacerFade(1f);

        
        // [AUDIO: Ambiente tenso / Música de los malos]
        fondoUchu.SetActive(true);
        yield return HacerFade(0f); 

        
        foreach (GameObject dialogo in dialogosConversacion)
        {
            dialogo.SetActive(true);
            // [AUDIO: Blip de texto apareciendo]
            yield return new WaitForSeconds(tiempoPorDialogo);
            dialogo.SetActive(false);
        }

        
        if(hudCamaraPOV != null) hudCamaraPOV.SetActive(true);
        // [AUDIO: Sonido de preparar cámara]
        
        yield return EjecutarQTE(400f, 135f, 60f, new Vector2(0, -100), false, tiempoLimiteQTE);
        
        if(hudCamaraPOV != null) hudCamaraPOV.SetActive(false);
        yield return new WaitForSeconds(0.5f); 

        
        yield return HacerFade(1f);
        fondoUchu.SetActive(false);
        // [AUDIO: Regresa el ambiente de la calle normal]

       
        yield return HacerFade(0f); 
        yield return new WaitForSeconds(1f);

        
        yield return MoverCamara(encuadreMoto.position, 0.3f); 
        
        // [AUDIO: Motor de moto arrancando y acelerando]
        imagenMoto.gameObject.SetActive(true);
        yield return MoverVehiculo(imagenMoto, destinoMoto.position, velocidadMoto);
        imagenMoto.gameObject.SetActive(false); 

        
        // Si tienes animación de caída ponla aquí, si no, usamos Idle. Baja muy rápido (x3).
        yield return MoverActor(actorCarrillo, animCarrillo, puntoAterrizaje.position, "Idle", velocidadTrepar * 3f); 
        // [AUDIO: Sonido de zapatos golpeando el pavimento al caer]
        
        
        // [AUDIO: Pasos corriendo INICIO]
        yield return MoverActor(actorCarrillo, animCarrillo, puntoSalida.position, "Correr", velocidadCaminar * 1.5f); 
        // [AUDIO: Pasos corriendo FIN]

        
        yield return HacerFade(1f);
        SceneManager.LoadScene(siguienteEscena);
    }

    

    IEnumerator MoverActor(Transform actor, Animator anim, Vector3 destino, string animName, float vel)
    {
        anim.Play(animName); 
        while (Vector2.Distance(actor.position, destino) > 0.1f)
        {
            Vector3 destinoConZ = new Vector3(destino.x, destino.y, actor.position.z);
            actor.position = Vector3.MoveTowards(actor.position, destinoConZ, vel * Time.deltaTime);
            yield return null; 
        }
        anim.Play("Idle");
    }

    IEnumerator SubirMuro(Transform actor, Vector3 destinoArriba, float vel)
    {
        while (Vector2.Distance(actor.position, destinoArriba) > 0.1f)
        {
            Vector3 destinoConZ = new Vector3(destinoArriba.x, destinoArriba.y, actor.position.z);
            actor.position = Vector3.MoveTowards(actor.position, destinoConZ, vel * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator MoverVehiculo(Transform vehiculo, Vector3 destino, float velocidad)
    {
        while (Vector2.Distance(vehiculo.position, destino) > 0.1f)
        {
            Vector3 destinoConZ = new Vector3(destino.x, destino.y, vehiculo.position.z);
            vehiculo.position = Vector3.MoveTowards(vehiculo.position, destinoConZ, velocidad * Time.deltaTime);
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
        
        if (targetAlpha == 0f) transitionPanel.SetActive(false);
    }

    IEnumerator EjecutarQTE(float vel, float angulo, float tamaño, Vector2 pos, bool esInfinito, float tiempoLimite)
    {
        bool qteResuelto = false;

        while (!qteResuelto)
        {
            bool intentoTerminado = false;
            bool exitoQTE = false;
            float tiempoActual = 0f; 

            qteUI.StartQTE(vel, angulo, tamaño, pos, 
                () => { exitoQTE = true; intentoTerminado = true; }, 
                () => { exitoQTE = false; intentoTerminado = true; });

            while (!intentoTerminado)
            {
                if (tiempoLimite > 0f)
                {
                    tiempoActual += Time.unscaledDeltaTime; 
                    if (tiempoActual >= tiempoLimite)
                    {
                        qteUI.gameObject.SetActive(false); 
                        exitoQTE = false;
                        intentoTerminado = true;
                    }
                }
                yield return null; 
            }

            if (exitoQTE)
            {
                // [AUDIO: ¡Click perfecto de la cámara!]
                qteResuelto = true; 
            }
            else
            {
                // [AUDIO: ¡Fallo de foto!]
                if (esInfinito)
                {
                    qteUI.gameObject.SetActive(false);
                    yield return new WaitForSeconds(1f); 
                }
                else
                {
                    qteResuelto = true; 
                }
            }
        }
    }
}