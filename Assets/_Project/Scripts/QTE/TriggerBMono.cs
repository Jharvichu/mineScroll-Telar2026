using Player;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerBMono : MonoBehaviour
{

    public GameObject characterp;
    public GameObject popupF;

    public MonkeyMove mono;
    public Transform puntoC;

    private bool esperandoF = false;
    private PlayerController controller;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Character"))
        {
            controller = characterp.GetComponent<PlayerController>();

            if (controller != null)
            {
                controller.canControl = false; // 🔥 PLAYER NO SE MUEVE
                controller.Rigidbody2D.linearVelocity = Vector2.zero;
            }

            popupF.SetActive(true);
            AudioManager.Instance.SetBGMParameter("QuickTimeEvent", 1f);
            esperandoF = true;
        }
    }

    void Update()
    {
        if (esperandoF && Input.GetKeyDown(KeyCode.F))
        {
            popupF.SetActive(false);
            AudioManager.Instance.SetBGMParameter("QuickTimeEvent", 0f);

            mono.IrA(puntoC, true); // va a C y se destruye

            if (controller != null)
            {
                controller.canControl = true; // 🔥 vuelve el control
            }

            esperandoF = false;
            Destroy(gameObject); // SOLO UNA VEZ
        }
    }
}