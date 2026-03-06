using FMODUnity;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static System.TimeZoneInfo;


public class CreditsController : MonoBehaviour
{

    [SerializeField] EventReference select;
    [SerializeField] EventReference back;
    public GameObject transition;
    public float transitionTime = 1f;
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        root.Q<Button>("back-button").clicked += () =>
        {
            AudioManager.Instance.PlaySFX(back);
            StartCoroutine(Transicion("MainMenu"));
        };
        IEnumerator Transicion(string escena)
        {
            transition.SetActive(true);

            CanvasGroup canvasGroup = transition.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
                canvasGroup = transition.AddComponent<CanvasGroup>();

            float elapsed = 0f;
            canvasGroup.alpha = 0f;

            // FADE IN
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
}

