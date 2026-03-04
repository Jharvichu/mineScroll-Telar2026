using UnityEngine;

public class TriggerAMono : MonoBehaviour
{

    public MonkeyMove mono;
    public Transform puntoB;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Character"))
        {
            mono.IrA(puntoB);
            Destroy(gameObject); // SOLO UNA VEZ
        }
    }
}