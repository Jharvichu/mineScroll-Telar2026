using UnityEngine;

public class MoverLancha : MonoBehaviour
{

    [Header("Punto de destino")]
    public Transform puntoDestino; // Arrastra aquí el objeto o punto al que quieres ir

    [Header("Velocidad de movimiento")]
    public float velocidad = 5f; // Editable desde el inspector

    void Update()
    {
        if (puntoDestino != null)
        {
            // Calcula la dirección y mueve el objeto
            transform.position = Vector3.MoveTowards(
                transform.position,           // posición actual
                puntoDestino.position,        // posición objetivo
                velocidad * Time.deltaTime    // distancia a mover por frame
            );
        }
    }
}
