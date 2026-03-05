using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class TransicionBlack : MonoBehaviour
{
    public Animator romboAnimator; // Tu animador de rombos
    public Image fadeImage; // Panel negro
    public float fadeDuration = 1f; // Duración del fade

    public void ChangeScene(string sceneName)
    {
        StartCoroutine(DoTransition(sceneName));
    }

    private IEnumerator DoTransition(string sceneName)
    {
        // 1. Reproducir animación de rombos
        if (romboAnimator != null)
        {
            romboAnimator.SetTrigger("Start"); // Usa el trigger que tengas en tu Animator
            // Espera la duración de la animación
            yield return new WaitForSeconds(romboAnimator.GetCurrentAnimatorStateInfo(0).length);
        }

        // 2. Fade a negro
        if (fadeImage != null)
        {
            float t = 0f;
            Color color = fadeImage.color;
            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                color.a = Mathf.Lerp(0, 1, t / fadeDuration);
                fadeImage.color = color;
                yield return null;
            }
        }

        // 3. Cambiar escena
        SceneManager.LoadScene(sceneName);
    }
}