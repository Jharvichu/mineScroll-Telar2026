                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System.Collections;
using FMODUnity;

public class MainMenuController : MonoBehaviour
{
    public GameObject transition;
    public float transitionTime = 1f;

    //AUDIO
    [SerializeField] EventReference select;
    [SerializeField] EventReference back;
    [SerializeField] EventReference hover1;
    [SerializeField] EventReference hover2;
    [SerializeField] EventReference hover3;

    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        root.Q<Button>("PlayButton").RegisterCallback<MouseEnterEvent>(_ => AudioManager.Instance.PlaySFX(hover1));

        root.Q<Button>("PlayButton").clicked += () =>
        {
            AudioManager.Instance.PlaySFX(select);
            StartCoroutine(Transicion("PrologoScene"));
        };

        root.Q<Button>("CreditsButton").RegisterCallback<MouseEnterEvent>(_ => AudioManager.Instance.PlaySFX(hover3));

        root.Q<Button>("CreditsButton").clicked += () =>
        {
            AudioManager.Instance.PlaySFX(select);
            StartCoroutine(Transicion("CreditsScene"));
        };

        root.Q<Button>("ControlsButton").RegisterCallback<MouseEnterEvent>(_ => AudioManager.Instance.PlaySFX(hover2));
        root.Q<Button>("ControlsButton").clicked += () =>
        {
            AudioManager.Instance.PlaySFX(select);
            StartCoroutine(Transicion("ControlsScene"));
        };

     
    }

    IEnumerator Transicion(string escena)
    {
        transition.SetActive(true);

        // Obtenemos el CanvasGroup del panel de transición
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

        // Cargamos la escena
        SceneManager.LoadScene(escena);
    }
}