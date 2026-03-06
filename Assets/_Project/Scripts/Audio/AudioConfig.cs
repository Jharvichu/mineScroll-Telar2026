using UnityEngine;
using FMODUnity;

[CreateAssetMenu(fileName = "AudioConfig", menuName = "Audio/Config")]
public class AudioConfig : ScriptableObject {
    public EventReference bgmEventA;
    public EventReference bgmEventB;
    public EventReference ambienceEventA;
    public EventReference ambienceEventB;
    public float defaultFadeTime = 2f;
}
