using FMODUnity;
using UnityEngine;

public class ZonaMusicaBorde : MonoBehaviour {
    [SerializeField] string parametro = "jungla_o_campamento"; // string directo, sin [ParamRef]
    [SerializeField] float valorIzquierda = 0f;
    [SerializeField] float valorDerecha = 1f;

    void OnTriggerEnter2D(Collider2D other) {
        if (!other.CompareTag("Character")) return;

        bool estaALaIzquierda = other.transform.position.x > transform.position.x;
        float valor = estaALaIzquierda ? valorIzquierda : valorDerecha;
        Debug.Log($"[ZonaBorde] {parametro} = {valor}");
        AudioManager.Instance.SetBGMParameter(parametro, valor);
    }
}
