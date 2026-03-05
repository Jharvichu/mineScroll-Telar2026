using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CinematicPrologue : MonoBehaviour
{
    [Header("Actores y Cámaras")]
    public Transform actorCarrillo;
    public Animator animCarrillo;
    public Transform actorCamioneta;
    public Animator animCamioneta;
    public Camera mainCamera;
    
    [Header("Interfaz QTE")]
    public CircularQTE qteUI;

    [Header("Marcadores de Movimiento (Waypoints)")]
    public Transform punto1;
    public Transform punto2;
    public Transform punto3;
    public Transform puntoFinal;

    [Header("Configuraciones")]
    public float velocidadCaminar = 3f;
    private float tamañoCamaraOriginal;
    private Vector3 posicionCamaraOriginal;

    
    private bool intentoTerminado = false;
    private bool exitoQTE = false;

    void Start()
    {
        tamañoCamaraOriginal = mainCamera.orthographicSize;
        posicionCamaraOriginal = mainCamera.transform.position;
        
        
        StartCoroutine(SecuenciaCinematica());
    }

    IEnumerator SecuenciaCinematica()
    {
        
        yield return MoverActor(actorCarrillo, animCarrillo, punto1.position);

        
        yield return EjecutarQTE(150f, 90f, 45f, new Vector2(0, 0), true);

        
        yield return MoverActor(actorCarrillo, animCarrillo, punto2.position);

        
        yield return EjecutarQTE(180f, 200f, 30f, new Vector2(150, 100), true);

       
        yield return MoverActor(actorCarrillo, animCarrillo, punto3.position);
        actorCarrillo.localScale = new Vector3(-1, 1, 1); 
        animCarrillo.Play("Idle");

        
        yield return MoverCamara(actorCamioneta.position, 0.5f); 
        animCamioneta.Play("Camioneta_Acercandose"); 
        
        
        yield return new WaitForSeconds(1.5f);
        actorCarrillo.gameObject.SetActive(false); 
        yield return MoverCamara(posicionCamaraOriginal, 0.5f);

        
        yield return new WaitForSeconds(1f); 
        
        mainCamera.orthographicSize = tamañoCamaraOriginal - 2f; 
        Time.timeScale = 0.3f; 
        
        
        yield return EjecutarQTE(400f, 45f, 50f, new Vector2(0, -100), false);
        
        
        Time.timeScale = 1f;
        mainCamera.orthographicSize = tamañoCamaraOriginal;
        
        
        yield return new WaitForSeconds(1.5f); 

        
        actorCarrillo.gameObject.SetActive(true);
        actorCarrillo.localScale = new Vector3(1, 1, 1);
        yield return MoverActor(actorCarrillo, animCarrillo, puntoFinal.position);

        
        Debug.Log("Cinemática Terminada. Cargando Siguiente Nivel...");
        // SceneManager.LoadScene("");
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

    IEnumerator EjecutarQTE(float vel, float angulo, float tamaño, Vector2 pos, bool esInfinito)
    {
        bool qteResuelto = false;
        animCarrillo.Play("Foto");

        while (!qteResuelto)
        {
            intentoTerminado = false;
            exitoQTE = false;

            
            qteUI.StartQTE(vel, angulo, tamaño, pos, 
                () => { exitoQTE = true; intentoTerminado = true; }, 
                () => { exitoQTE = false; intentoTerminado = true; });

            
            yield return new WaitUntil(() => intentoTerminado);

            if (exitoQTE)
            {
                qteResuelto = true; 
            }
            else
            {
                if (esInfinito)
                {
                    animCarrillo.Play("Idle"); 
                    yield return new WaitForSeconds(1f); 
                    animCarrillo.Play("Foto"); 
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