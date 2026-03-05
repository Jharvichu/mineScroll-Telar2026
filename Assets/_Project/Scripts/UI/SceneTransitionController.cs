using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Player;

public class SceneTransitionController : MonoBehaviour
{
    public static SceneTransitionController Instance { get; private set; }

    [Header("References")]
    [SerializeField] private PlayerController player;
    [SerializeField] private Image fadeImage;

    [Header("Settings")]
    [SerializeField] private float transitionDuration = 2.0f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        EnsureImageReference();
    }

    private void Start()
    {
        FadeIn();
    }

    private void EnsureImageReference()
    {
        if (fadeImage == null)
        {
            fadeImage = GetComponent<Image>();
        }
    }

    public void FadeIn() => StartCoroutine(PerformTransition(1f, 0f));

    public void FadeOut() => StartCoroutine(PerformTransition(0f, 1f));

    private IEnumerator PerformTransition(float startAlpha, float endAlpha)
    {
        if (player != null) player.canControl = false;

        float elapsedTime = 0f;
        Color tempColor = fadeImage.color;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = elapsedTime / transitionDuration;

            tempColor.a = Mathf.Lerp(startAlpha, endAlpha, normalizedTime);
            fadeImage.color = tempColor;

            yield return null;
        }

        tempColor.a = endAlpha;
        fadeImage.color = tempColor;

        if (endAlpha == 0f && player != null) player.canControl = true;

    }
}
