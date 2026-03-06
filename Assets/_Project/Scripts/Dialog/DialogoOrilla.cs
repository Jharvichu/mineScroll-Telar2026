using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class DialogoOrilla : MonoBehaviour
{

    [System.Serializable]
    public class DialogueImage
    {
        public Sprite image;       // La imagen que se mostrará
        public float duration = 2f; // Tiempo que se verá la imagen
        public bool isCharacterA;
    }

    public OrillaaTuto cambioEscena; // Tu referencia para cambiar escena

    [Header("UI References")]
    public GameObject panelA;
    public GameObject panelB;
    public Image imageA;
    public Image imageB;

    [Header("Diálogos en imágenes")]
    public DialogueImage[] dialogueLines;

    private void Start()
    {
        panelA.SetActive(false);
        panelB.SetActive(false);
        StartCoroutine(PlayDialogue());
    }

    IEnumerator PlayDialogue()
    {
        AudioManager.Instance.SetBGMParameter("activar_ambiente", 1f);

        
        foreach (DialogueImage line in dialogueLines)
        {
            if (line.isCharacterA)
                yield return StartCoroutine(ShowImage(panelA, imageA, line.image, line.duration));
            else
                yield return StartCoroutine(ShowImage(panelB, imageB, line.image, line.duration));
        }

        Debug.Log("Diálogo terminado");
        cambioEscena.IrATutorial();
    }

    IEnumerator ShowImage(GameObject panel, Image imageUI, Sprite sprite, float duration)
    {
        panelA.SetActive(false);
        panelB.SetActive(false);

        panel.SetActive(true);
        imageUI.sprite = sprite;

        yield return new WaitForSeconds(duration);

        panel.SetActive(false);
    }
}
