using UnityEngine;
using FMODUnity;

public class EnemyCuchilloAudio : MonoBehaviour {
    [SerializeField] EventReference footstepEvent;
    [SerializeField] EventReference swingEvent;

    public void OnFootstep() {
        AudioManager.Instance.PlaySFX3D(footstepEvent, transform.position);
    }
    public void OnSwing() {
        AudioManager.Instance.PlaySFX3D(swingEvent, transform.position);
    }

}