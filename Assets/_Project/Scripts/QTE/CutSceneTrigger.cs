using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Player;

public class CutSceneTrigger : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject characterp;     
    public GameObject popupF;         

    [Header("Opciones de QTE")]
    public bool hasTimeLimit = false; 
    public float qteDuration = 2f;

    [Header("UI")]
    public Text timerText;

    private bool inCutscene = false;  
    private bool waitingForPhoto = false;
    private float qteTimer = 0f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Character") && !inCutscene)
        {
            StartCutscene();
        }
    }
    void StartCutscene()
    {
        inCutscene = true;

        PlayerController controller = characterp.GetComponent<PlayerController>();

        if (controller != null)
        {
            controller.canControl = false;
            controller.Rigidbody2D.linearVelocity = Vector2.zero;


        }

        Debug.Log("Cutscene iniciada");
        Invoke(nameof(ShowPhotoPopup), 2f);
    }
    void ShowPhotoPopup()
    {
        popupF.SetActive(true);
        waitingForPhoto = true;
        qteTimer = 0f; 
        Debug.Log("Momento de tomar la foto");
    }
    void Update()
    {
        if (waitingForPhoto)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                TakePhoto();
            }
            else if (hasTimeLimit)
            {
                qteTimer += Time.deltaTime;

                float timeLeft = qteDuration - qteTimer;
                timerText.text = "Tiempo: " + timeLeft.ToString("F1");

                if (qteTimer >= qteDuration)
                {
                    Debug.Log("No presionó F a tiempo. Reiniciando escena");
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
            }
        }
    }
    void TakePhoto()
    {
        waitingForPhoto = false;
        popupF.SetActive(false);

        if (timerText != null)
            timerText.text = "";

        Destroy(gameObject);

        Debug.Log("Foto tomada");
        EndCutscene();
    }
    void EndCutscene()
    {
        PlayerController controller = characterp.GetComponent<PlayerController>();

        if (controller != null)
        {
            controller.canControl = true;
        }

        inCutscene = false;
        Debug.Log("Cutscene finalizada");
    }
}