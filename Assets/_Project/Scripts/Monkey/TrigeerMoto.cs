using Player;
using System.Collections.Generic;
using UnityEngine;

public class TrigeerMoto : MonoBehaviour
{
    public List<MotoMove> motos = new List<MotoMove>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Character"))
        {
            PlayerController controller = other.GetComponent<PlayerController>();

            if (controller != null)
            {
                controller.canControl = false;
                controller.Rigidbody2D.linearVelocity = Vector2.zero;
                controller.Rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
            }

            // 🔥 Activar TODAS las motos
            foreach (MotoMove moto in motos)
            {
                moto.ActivateMoto(controller);
            }

            Destroy(gameObject);
        }
    }
}