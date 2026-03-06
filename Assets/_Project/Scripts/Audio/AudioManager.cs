using System.Collections;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour {
    public static AudioManager Instance { get; private set; }

    AudioConfig config;

    EventInstance bgmA;
    EventInstance bgmB;
    EventInstance ambienceA;
    EventInstance ambienceB;

    EventInstance activeBGM;
    EventInstance activeAmbience;

    FMOD.GUID activeBGMGuid;
    FMOD.GUID activeAmbienceGuid;

    // Se ejecuta automáticamente antes de cargar cualquier escena
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void AutoCreate() {
        if (Instance != null) return;
        GameObject go = new GameObject("AudioManager");
        go.AddComponent<AudioManager>();
    }

    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        config = Resources.Load<AudioConfig>("AudioConfig");

        if (config == null)
            Debug.LogError("AudioConfig no encontrado en Resources!");
    }

    void Start() {
        if (config == null) return;

        if (!config.bgmEventA.IsNull) { bgmA = RuntimeManager.CreateInstance(config.bgmEventA); bgmA.setVolume(1f); bgmA.start(); }
        if (!config.bgmEventB.IsNull) { bgmB = RuntimeManager.CreateInstance(config.bgmEventB); bgmB.setVolume(0f); }
        if (!config.ambienceEventA.IsNull) { ambienceA = RuntimeManager.CreateInstance(config.ambienceEventA); ambienceA.setVolume(1f); ambienceA.start(); }
        if (!config.ambienceEventB.IsNull) { ambienceB = RuntimeManager.CreateInstance(config.ambienceEventB); ambienceB.setVolume(0f); }

        activeBGM = bgmA;
        activeAmbience = ambienceA;
    }

    void OnDestroy() {
        StopAndRelease(bgmA);
        StopAndRelease(bgmB);
        StopAndRelease(ambienceA);
        StopAndRelease(ambienceB);
    }

    void StopAndRelease(EventInstance instance) {
        if (instance.isValid()) {
            instance.stop(STOP_MODE.ALLOWFADEOUT);
            instance.release();
        }
    }

    // ── BGM ──────────────────────────────────

    public void SetBGMParameter(string param, float value) {
        if (activeBGM.isValid())
            activeBGM.setParameterByName(param, value);
    }

    public void CrossfadeBGM(EventReference newEvent, float fadeTime = -1f) {
        // Si es el mismo evento, no hacer nada
        if (newEvent.Guid == activeBGMGuid) return;

        float duration = fadeTime < 0 ? config.defaultFadeTime : fadeTime;
        EventInstance next = RuntimeManager.CreateInstance(newEvent);
        StartCoroutine(CrossfadeRoutine(activeBGM, next, duration));

        activeBGM = next;
        activeBGMGuid = newEvent.Guid; // guarda el guid del evento activo
    }

    // ── AMBIENCE ─────────────────────────────

    public void SetAmbienceParameter(string param, float value) {
        if (activeAmbience.isValid())
            activeAmbience.setParameterByName(param, value);
    }

    public void CrossfadeAmbience(EventReference newEvent, float fadeTime = -1f) {
        if (newEvent.IsNull) return;
        if (newEvent.Guid == activeAmbienceGuid) return; // mismo evento, no crossfadear

        float duration = fadeTime < 0 ? config.defaultFadeTime : fadeTime;
        EventInstance next = RuntimeManager.CreateInstance(newEvent);
        StartCoroutine(CrossfadeRoutine(activeAmbience, next, duration));

        activeAmbience = next;
        activeAmbienceGuid = newEvent.Guid;
    }

    // ── CROSSFADE ────────────────────────────

    IEnumerator CrossfadeRoutine(EventInstance current, EventInstance next, float duration) {
        next.setVolume(0f);
        next.start();

        float elapsed = 0f;
        while (elapsed < duration) {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            current.setVolume(1f - t);
            next.setVolume(t);
            yield return null;
        }

        current.stop(STOP_MODE.ALLOWFADEOUT);
        current.setVolume(1f);
    }

    // ── SFX ──────────────────────────────────

    public void PlaySFX(EventReference sfxEvent)
        => RuntimeManager.PlayOneShot(sfxEvent);

    public void PlaySFX3D(EventReference sfxEvent, Vector3 position)
        => RuntimeManager.PlayOneShot(sfxEvent, position);

    public EventInstance GetBGMInstance() {
        return activeBGM;
    }

}
