using System.Collections;
using UnityEngine;
using TMPro;
public class DialogoOrilla : MonoBehaviour
{
    [System.Serializable]
    public class DialogueLine
    {
        public bool isCharacterA; 
        [TextArea(3, 5)]
        public string text;
    }

    [Header("UI References")]
    public GameObject panelA;
    public GameObject panelB;
    public TextMeshProUGUI textA;
    public TextMeshProUGUI textB;

    [Header("Configuración")]
    public float typingSpeed = 0.05f;
    public float visibleDuration = 2f; 

    [Header("Diálogos")]
    public DialogueLine[] dialogueLines;

    private void Start()
    {
        panelA.SetActive(false);
        panelB.SetActive(false);
        StartCoroutine(PlayDialogue());
    }

    IEnumerator PlayDialogue()
    {
        AudioManager.Instance.SetBGMParameter("activar_ambiente", 1f);

        foreach (DialogueLine line in dialogueLines)
        {
            if (line.isCharacterA)
            {
                yield return StartCoroutine(ShowLine(panelA, textA, line.text));
            }
            else
            {
                yield return StartCoroutine(ShowLine(panelB, textB, line.text));
            }
        }

        Debug.Log("Diálogo terminado");
    }

    IEnumerator ShowLine(GameObject panel, TextMeshProUGUI textUI, string message)
    {
        panelA.SetActive(false);
        panelB.SetActive(false);

        panel.SetActive(true);
        textUI.text = "";

        foreach (char letter in message)
        {
            textUI.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        yield return new WaitForSeconds(visibleDuration);

        panel.SetActive(false);
    }
}
