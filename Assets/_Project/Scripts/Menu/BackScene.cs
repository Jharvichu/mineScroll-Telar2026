using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using FMODUnity;

public class CreditsController : MonoBehaviour
{

    [SerializeField] EventReference select;
    [SerializeField] EventReference back;
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        root.Q<Button>("back-button").clicked += () =>
        {
            AudioManager.Instance.PlaySFX(back);
            SceneManager.LoadScene("MainMenu");
        };
    }
}
