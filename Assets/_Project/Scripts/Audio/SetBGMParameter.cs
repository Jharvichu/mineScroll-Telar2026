using UnityEngine;
using FMODUnity;

public class SetBGMParameter : MonoBehaviour {
    [ParamRef][SerializeField] string parameterName;
    [SerializeField] float value;

    void Start() {
        AudioManager.Instance.SetBGMParameter("activar_ambiente", 1f);
        Destroy(gameObject);
    }
}
