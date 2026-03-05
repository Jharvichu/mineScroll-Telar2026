using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;

public class CinematicPrologue : MonoBehaviour
{
    [Header("Actores y Cámaras")]
    public Transform actorCarrillo;
    public Animator animCarrillo;
    public Transform actorCamioneta;
    public Animator animCamioneta;
    public Camera mainCamera;

    [Header("SFX Cámara")]
    [SerializeField] EventReference sfxPrepareCamera;
    [SerializeField] EventReference sfxCameraShot;
    [SerializeField] EventReference sfxCameraShotFail;

    [Header("SFX Camioneta")]
    [SerializeField] EventReference sfxCamioneta;

    private FMOD.Studio.EventInstance instanciaCamioneta;


    [Header("Interfaz y POV")]
    public CircularQTE qteUI;
    public GameObject hudCamaraPOV; 

    [Header("Marcadores de Movimiento (Carrillo)")]
    public Transform punto1;
    public Transform punto2;
    public Transform punto3;
    public Transform puntoFinal;

    [Header("Encuadres y Camioneta")]
    public Transform encuadre1; 
    public Transform encuadre2; 
    public Transform encuadreViendoCamioneta; 
    public Transform encuadreCamioneta; 
    public Transform destinoCamioneta; 

    [Header("Configuraciones")]
    public float velocidadCaminar = 3f;
    public float velocidadCamioneta = 15f; 
    
    [Tooltip("Segundos reales antes de fallar el QTE de la camioneta")]
    public float tiempoLimiteCamioneta = 1.5f; 

    [Header("Transición Final")]
    public GameObject transitionPanel; 
    public float transitionTime = 1f;
    public string siguienteEscena = "Nivel2"; 

    private float tamañoCamaraOriginal;
    private Vector3 posicionCamaraOriginal;
    private Vector3 escalaOriginalCarrillo; 

    private bool intentoTerminado = false;
    private bool exitoQTE = false;

    void Start()
    {
        tamañoCamaraOriginal = mainCamera.orthographicSize;
        posicionCamaraOriginal = mainCamera.transform.position;
        escalaOriginalCarrillo = actorCarrillo.localScale; 
        
        if (hudCamaraPOV != null) hudCamaraPOV.SetActive(false);
        if (transitionPanel != null) transitionPanel.SetActive(false); 
        
        StartCoroutine(SecuenciaCinematica());
    }

    IEnumerator SecuenciaCinematica()
    {
        
        yield return MoverActor(actorCarrillo, animCarrillo, punto1.position);
        animCarrillo.Play("Foto");
        AudioManager.Instance.PlaySFX(sfxPrepareCamera);
        if (encuadre1 != null) yield return MoverCamara(encuadre1.position, 0.5f);
        if(hudCamaraPOV != null) hudCamaraPOV.SetActive(true);
        yield return EjecutarQTE(150f, 90f, 45f, new Vector2(0, 0), true, 0f);
        if(hudCamaraPOV != null) hudCamaraPOV.SetActive(false);
        yield return MoverCamara(posicionCamaraOriginal, 0.5f);

        
        yield return MoverActor(actorCarrillo, animCarrillo, punto2.position);
        animCarrillo.Play("Foto");
        AudioManager.Instance.PlaySFX(sfxPrepareCamera);
        if (encuadre2 != null) yield return MoverCamara(encuadre2.position, 0.5f);
        if(hudCamaraPOV != null) hudCamaraPOV.SetActive(true);
        yield return EjecutarQTE(180f, 200f, 30f, new Vector2(150, 100), true, 0f);
        if(hudCamaraPOV != null) hudCamaraPOV.SetActive(false);
        yield return MoverCamara(posicionCamaraOriginal, 0.5f);

        
        yield return MoverActor(actorCarrillo, animCarrillo, punto3.position);
        actorCarrillo.localScale = new Vector3(-Mathf.Abs(escalaOriginalCarrillo.x), escalaOriginalCarrillo.y, escalaOriginalCarrillo.z); 
        animCarrillo.Play("Idle");

        
        if(encuadreViendoCamioneta != null) 
        {
            yield return MoverCamara(encuadreViendoCamioneta.position, 0.5f); 
        }
        else 
        {
            yield return MoverCamara(actorCamioneta.position, 0.5f); 
        }

        animCamioneta.Play("Camioneta_Acercandose");
        instanciaCamioneta = FMODUnity.RuntimeManager.CreateInstance(sfxCamioneta);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(
            instanciaCamioneta,
            actorCamioneta.gameObject
        );
        instanciaCamioneta.start();
        StartCoroutine(MoverVehiculo(actorCamioneta, destinoCamioneta.position, velocidadCamioneta));
        
        // 5.
        yield return new WaitForSeconds(1.5f);
        actorCarrillo.gameObject.SetActive(false); 
        yield return MoverCamara(posicionCamaraOriginal, 0.5f);

        // 6.
        yield return new WaitForSeconds(1f); 
        if(encuadreCamioneta != null) yield return MoverCamara(encuadreCamioneta.position, 0.2f);
        mainCamera.orthographicSize = tamañoCamaraOriginal - 2f; 
        Time.timeScale = 0.3f; 
        
        if(hudCamaraPOV != null) hudCamaraPOV.SetActive(true);
        animCarrillo.Play("Foto");
        AudioManager.Instance.PlaySFX(sfxPrepareCamera);
        yield return EjecutarQTE(400f, 135f, 60f, new Vector2(0, -100), false, tiempoLimiteCamioneta);
        if(hudCamaraPOV != null) hudCamaraPOV.SetActive(false);
        
        // 7.
        Time.timeScale = 1f;
        mainCamera.orthographicSize = tamañoCamaraOriginal;
        yield return MoverCamara(posicionCamaraOriginal, 0.5f);
        
        // 8.
        yield return new WaitForSeconds(1.5f); 
        actorCarrillo.gameObject.SetActive(true);

        actorCarrillo.localScale = new Vector3(Mathf.Abs(escalaOriginalCarrillo.x), escalaOriginalCarrillo.y, escalaOriginalCarrillo.z);
        yield return MoverActor(actorCarrillo, animCarrillo, puntoFinal.position);

        Debug.Log("Cinemática Terminada. Iniciando Transición Final...");


        instanciaCamioneta.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        instanciaCamioneta.release();

        if (transitionPanel != null)
        {
            transitionPanel.SetActive(true);
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
            yield return new WaitForSeconds(0.5f);
        }

        Debug.Log("Viajando al " + siguienteEscena + "...");
        SceneManager.LoadScene(siguienteEscena);
    }

    IEnumerator MoverActor(Transform actor, Animator anim, Vector3 destino)
    {
        anim.Play("Correr"); 
        while (Vector2.Distance(actor.position, destino) > 0.1f)
        {
            actor.position = Vector2.MoveTowards(actor.position, destino, velocidadCaminar * Time.deltaTime);
            yield return null; 
        }
        anim.Play("Idle");
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
        Vector3 posicionInicial = mainCamera.transform.position;
        Vector3 posicionFinal = new Vector3(destino.x, destino.y, posicionInicial.z); 
        float tiempo = 0;

        while (tiempo < duracionViaje)
        {
            mainCamera.transform.position = Vector3.Lerp(posicionInicial, posicionFinal, tiempo / duracionViaje);
            tiempo += Time.deltaTime;
            yield return null;
        }
        mainCamera.transform.position = posicionFinal;
    }

    IEnumerator EjecutarQTE(float vel, float angulo, float tamaño, Vector2 pos, bool esInfinito, float tiempoLimite)
    {
        bool qteResuelto = false;        
        animCarrillo.Play("CamaraPreparada");

        while (!qteResuelto)
        {
            intentoTerminado = false;
            exitoQTE = false;
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
                        Debug.Log("¡Se te escapó la camioneta!");
                        qteUI.gameObject.SetActive(false); 
                        exitoQTE = false;
                        intentoTerminado = true;
                    }
                }
                yield return null; 
            }

            if (exitoQTE)
            {
                AudioManager.Instance.PlaySFX(sfxCameraShot); // ← éxito
                qteResuelto = true; 
            }
            else
            {
                AudioManager.Instance.PlaySFX(sfxCameraShotFail); // ← fallo
                if (esInfinito)
                {
                    animCarrillo.Play("Idle"); 
                    qteUI.gameObject.SetActive(false);
                    yield return new WaitForSeconds(1f); 
                    animCarrillo.Play("Foto");
                    AudioManager.Instance.PlaySFX(sfxPrepareCamera);
                }
                else
                {
                    qteResuelto = true; 
                }
            }
        }
        animCarrillo.Play("Idle");
    }
}