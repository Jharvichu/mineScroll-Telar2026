using UnityEngine;
using FMODUnity;

public class SceneAudio : MonoBehaviour {
    [Header("BGM")]
    [SerializeField] EventReference bgmEvent;
    [SerializeField] float bgmFadeTime = 2f;

    [Header("Ambience")]
    [SerializeField] EventReference ambienceEvent;
    [SerializeField] float ambienceFadeTime = 2f;

    void Start() {
        if (!bgmEvent.IsNull)
            AudioManager.Instance.CrossfadeBGM(bgmEvent, bgmFadeTime);

        if (!ambienceEvent.IsNull)
            AudioManager.Instance.CrossfadeAmbience(ambienceEvent, ambienceFadeTime);
    }
}
