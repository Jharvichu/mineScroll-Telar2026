using UnityEngine;
using FMODUnity;

public class PlayerAudio : MonoBehaviour {
    [SerializeField] EventReference footstepEvent;
    [SerializeField] EventReference climbEvent;
    [SerializeField] EventReference heartbeatEvent;

    public void OnFootstep() {
        AudioManager.Instance.PlaySFX3D(footstepEvent, transform.position);
    }

    public void OnClimb() {
        AudioManager.Instance.PlaySFX3D(climbEvent, transform.position);
    }

    public void OnHide() {
        AudioManager.Instance.PlaySFX3D(heartbeatEvent, transform.position);
    }
}