using UnityEngine;
using FMODUnity;

public class PlayerAudio : MonoBehaviour {
    [SerializeField] EventReference footstepEvent;

    public void OnFootstep() {
        AudioManager.Instance.PlaySFX3D(footstepEvent, transform.position);
    }
}