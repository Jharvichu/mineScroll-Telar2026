using UnityEngine;

public class CutSceneTrigger : MonoBehaviour
{
    public GameObject characterp;     
    public GameObject popupF;      

    private bool inCutscene = false;
    private bool waitingForPhoto = false;

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

        
        Invoke(nameof(ShowPhotoPopup), 2f);
    }

    void ShowPhotoPopup()
    {
        popupF.SetActive(true);
        waitingForPhoto = true;
        Debug.Log("Momento de tomar la foto");
    }

    void Update()
    {
        if (waitingForPhoto && Input.GetKeyDown(KeyCode.F))
        {
            TakePhoto();
        }
    }

    void TakePhoto()
    {
        waitingForPhoto = false;
        popupF.SetActive(false);
        Debug.Log("Foto tomada");

        EndCutscene();
        Destroy(gameObject);
    }

    void EndCutscene()
    {
        characterp.GetComponent<CharacterMovement>().canMove = true;
        inCutscene = false;
        Debug.Log("Cutscene finalizada");
    }
}
