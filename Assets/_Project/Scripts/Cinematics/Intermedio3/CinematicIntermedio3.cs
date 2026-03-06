using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CinematicIntermedio3 : MonoBehaviour
{
    [Header("Actores Principales")]
    public Transform actorCarrillo;
    public Animator animCarrillo;
    public Transform actorCamioneta;
    public Animator animCamioneta;
    public Camera mainCamera;

    [Header("Extras (Los Malos y las Niñas)")]
    public Transform[] enemigos; 
    public Animator[] animEnemigos;
    public Transform[] ninas; 
    public Animator[] animNinas;
    
    [Header("La Niña Rescatada (ACTOR EN CAMERINO)")]
    public Transform ninaRescatada;
    public Animator animNinaRescatada;

    [Header("Interfaz y POV")]
    public CircularQTE qteUI;
    public GameObject hudCamaraPOV; 
    public float tiempoLimiteQTE = 2.0f;

    [Header("Encuadres de Cámara")]
    public Transform encuadreCamioneta; 
    public Transform encuadreQTE;       
    public Transform encuadreCielo; 

    [Header("Marcadores de Movimiento")]
    public Transform marcaCarrilloLlega; 
    public Transform marcaFrenoCamioneta; 
    public Transform marcaEscondite; 
    public Transform marcaFugaMalos; 
    public Transform marcaFrenteCamioneta; 
    public Transform marcaNinaSuelo; 
    public Transform marcaEscapeIzquierda; 

    [Header("Configuraciones")]
    public float velocidadCaminar = 3f;
    public float velocidadCorrer = 5f;
    public float velocidadCamioneta = 15f;

    [Header("Transiciones y Escena")]
    public GameObject transitionPanel;
    public float transitionTime = 1f;
    public string siguienteEscena = "Nivel4"; 

    private Vector3 posicionCamaraOriginal;

    void Start()
    {
        posicionCamaraOriginal = mainCamera.transform.position;
        
        if (hudCamaraPOV != null) hudCamaraPOV.SetActive(false);
        if (transitionPanel != null) transitionPanel.SetActive(false);
        
        // ¡MAGIA ELIMINADA! Ya no apagamos a la Niña Rescatada aquí. 
        // Ella está viva y feliz esperando en su camerino (fuera de cámara).
        foreach (var e in enemigos) e.gameObject.SetActive(false);
        foreach (var n in ninas) n.gameObject.SetActive(false);

        StartCoroutine(SecuenciaIntermedio3());
    }

    IEnumerator SecuenciaIntermedio3()
    {
        // 1. Carrillo llega
        yield return MoverActor(actorCarrillo, animCarrillo, marcaCarrilloLlega.position, "Correr", velocidadCaminar, "Idle");

        // 2. Se voltea
        VoltearActor(actorCarrillo, true); 
        yield return new WaitForSeconds(1f);

        // 3. Llega la camioneta
        yield return MoverCamara(encuadreCamioneta.position, 0.5f);
        animCamioneta.Play("Camioneta_Acercandose"); 
        yield return MoverVehiculo(actorCamioneta, marcaFrenoCamioneta.position, velocidadCamioneta);
        yield return new WaitForSeconds(0.5f);

        // 4. Carrillo se esconde
        actorCarrillo.position = marcaEscondite.position;
        VoltearActor(actorCarrillo, false); 
        animCarrillo.Play("Agacharse");
        yield return MoverCamara(posicionCamaraOriginal, 0.5f);
        yield return new WaitForSeconds(1f);

        // 5. Spawnean extras
        if (encuadreQTE != null) yield return MoverCamara(encuadreQTE.position, 0.5f);
        
        for (int i = 0; i < enemigos.Length; i++)
        {
            enemigos[i].gameObject.SetActive(true);
            StartCoroutine(MoverActor(enemigos[i], animEnemigos[i], marcaFugaMalos.position, "Caminar", velocidadCaminar, "Idle"));
        }
        for (int i = 0; i < ninas.Length; i++)
        {
            ninas[i].gameObject.SetActive(true);
            StartCoroutine(MoverActor(ninas[i], animNinas[i], marcaFugaMalos.position, "CorrerNina", velocidadCorrer, ""));
        }
        
        yield return new WaitForSeconds(0.5f);

        // 6. QTE
        if(hudCamaraPOV != null) hudCamaraPOV.SetActive(true);
        yield return EjecutarQTE(300f, 90f, 60f, new Vector2(0, 0), false, tiempoLimiteQTE);
        if(hudCamaraPOV != null) hudCamaraPOV.SetActive(false);

        // 7. Salen extras de pantalla
        yield return new WaitForSeconds(2.5f);
        foreach (var e in enemigos) e.gameObject.SetActive(false);
        foreach (var n in ninas) n.gameObject.SetActive(false);
        
        // 8. Carrillo sale a ayudar
        yield return MoverCamara(posicionCamaraOriginal, 0.5f);
        yield return MoverActor(actorCarrillo, animCarrillo, marcaFrenteCamioneta.position, "Correr", velocidadCaminar, "Idle");
        
        // 9. Cámara al cielo
        yield return MoverCamara(encuadreCielo.position, 0.5f);
        yield return new WaitForSeconds(0.5f);
        
       // 10. EL TRUCO DEL CARRO: ¡Teletransportamos a la niña desde el camerino!
        // Usamos la izquierda/derecha (X) de la marca, pero le ROBAMOS el piso (Y) a Carrillo para que no se hunda.
        ninaRescatada.position = new Vector3(marcaNinaSuelo.position.x, actorCarrillo.position.y, actorCarrillo.position.z);
        
        // 11. Cámara baja
        yield return MoverCamara(posicionCamaraOriginal, 0.5f);
        yield return new WaitForSeconds(0.5f);

        // 12. Se levanta
        animNinaRescatada.Play("LevantarseNina");
        yield return new WaitForSeconds(1.5f); 

        // 13. Huye a la IZQUIERDA
        VoltearActor(ninaRescatada, true);
        StartCoroutine(MoverActor(ninaRescatada, animNinaRescatada, marcaEscapeIzquierda.position, "CorrerNina", velocidadCorrer, ""));
        yield return new WaitForSeconds(0.3f); 

        // 14. Carrillo huye a la IZQUIERDA
        VoltearActor(actorCarrillo, true);
        yield return MoverActor(actorCarrillo, animCarrillo, marcaEscapeIzquierda.position, "Correr", velocidadCorrer, "Idle");

        // 15. FIN
        yield return HacerFade(1f);
        SceneManager.LoadScene(siguienteEscena);
    }

    // --- FUNCIONES LIMPIAS (Ignoran Eje Y) ---

    IEnumerator MoverActor(Transform actor, Animator anim, Vector3 destino, string animName, float vel, string animCierre)
    {
        if(anim != null && anim.gameObject.activeInHierarchy) anim.Play(animName); 
        
        while (Mathf.Abs(actor.position.x - destino.x) > 0.1f)
        {
            Vector3 destinoSeguro = new Vector3(destino.x, actor.position.y, actor.position.z);
            actor.position = Vector3.MoveTowards(actor.position, destinoSeguro, vel * Time.deltaTime);
            yield return null; 
        }
        
        if(anim != null && !string.IsNullOrEmpty(animCierre) && anim.gameObject.activeInHierarchy) anim.Play(animCierre);
    }

   IEnumerator MoverVehiculo(Transform vehiculo, Vector3 destino, float velocidad)
    {
        while (Mathf.Abs(vehiculo.position.x - destino.x) > 0.1f)
        {
            Vector3 destinoSeguro = new Vector3(destino.x, vehiculo.position.y, vehiculo.position.z);
            vehiculo.position = Vector3.MoveTowards(vehiculo.position, destinoSeguro, velocidad * Time.deltaTime);
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
                qteResuelto = true; 
            }
            else
            {
                if (esInfinito)
                {
                    qteUI.gameObject.SetActive(false);
                    yield return new WaitForSeconds(1f); 
                }
                else qteResuelto = true; 
            }
        }
    }

    void VoltearActor(Transform actor, bool mirarIzquierda)
    {
        Vector3 escala = actor.localScale;
        escala.x = mirarIzquierda ? -Mathf.Abs(escala.x) : Mathf.Abs(escala.x);
        actor.localScale = escala;
    }
}