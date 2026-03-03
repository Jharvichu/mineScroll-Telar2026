using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System.Collections;

public class MainMenuController : MonoBehaviour
{
    public GameObject transition;
    public float transitionTime = 1f;

    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        root.Q<Button>("PlayButton").clicked += () =>
        {
            StartCoroutine(Transicion("PrologoScene"));
        };

        root.Q<Button>("CreditsButton").clicked += () =>
        {
            StartCoroutine(Transicion("CreditsScene"));
        };

        root.Q<Button>("ControlsButton").clicked += () =>
        {
            StartCoroutine(Transicion("ControlsScene"));
        };

        root.Q<Button>("ExitButton").clicked += () =>
        {
            Application.Quit();
            Debug.Log("Salí del juego");
        };
    }

    IEnumerator Transicion(string escena)
    {
        transition.SetActive(true);
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(escena);
    }
}