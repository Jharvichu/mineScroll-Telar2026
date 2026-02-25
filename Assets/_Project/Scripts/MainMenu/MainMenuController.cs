using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuController : MonoBehaviour
{
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        root.Q<Button>("PlayButton").clicked += () =>
        {
            SceneManager.LoadScene("GameScene");
        };

        root.Q<Button>("CreditsButton").clicked += () =>
        {
            SceneManager.LoadScene("CreditsScene");
        };

        root.Q<Button>("ControlsButton").clicked += () =>
        {
            SceneManager.LoadScene("ControlsScene");
        };

        root.Q<Button>("ExitButton").clicked += () =>
        {
            Application.Quit();
            Debug.Log("Salí del juego");
        };
    }
}
