using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class OrillaaTuto : MonoBehaviour
{
    public GameObject transition;
    public float transitionTime = 1f;

    public void IrATutorial()
    {
        StartCoroutine(Transicion("Tutorial 1"));
    }

    IEnumerator Transicion(string escena)
    {
        transition.SetActive(true);

        CanvasGroup canvasGroup = transition.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = transition.AddComponent<CanvasGroup>();

        float elapsed = 0f;
        canvasGroup.alpha = 0f;

        // FADE
        while (elapsed < transitionTime)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsed / transitionTime);
            yield return null;
        }

        canvasGroup.alpha = 1f;

        SceneManager.LoadScene(escena);
    }
}