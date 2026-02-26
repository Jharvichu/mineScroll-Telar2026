using UnityEngine;
using UnityEngine.SceneManagement;

public class CutSceneTrigger : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject characterp;     
    public GameObject popupF;         

    [Header("Opciones de QTE")]
    public bool hasTimeLimit = false; 
    public float qteDuration = 2f;   

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

        characterp.GetComponent<CharacterMovement>().canMove = false;

        Debug.Log("Cutscene iniciada");

        // Delay hasta el momento de la foto
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
        Destroy(gameObject);

        Debug.Log("Foto tomada");
        EndCutscene();
    }

    void EndCutscene()
    {
        characterp.GetComponent<CharacterMovement>().canMove = true;
        inCutscene = false;
        Debug.Log("Cutscene finalizada");
    }
}