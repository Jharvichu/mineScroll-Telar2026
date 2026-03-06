using UnityEngine;
using FMODUnity;

public class PlayerAudio : MonoBehaviour {
    [SerializeField] EventReference footstepEvent;
    [SerializeField] EventReference climbEvent;
    [SerializeField] EventReference heartbeatEvent;
    [SerializeField] EventReference ledgegrabEvent;
    [SerializeField] EventReference godownEvent;
    [SerializeField] EventReference deathEvent;

    public void OnFootstep() {
        AudioManager.Instance.PlaySFX3D(footstepEvent, transform.position);
    }

    public void OnClimb() {
        AudioManager.Instance.PlaySFX3D(climbEvent, transform.position);
    }

    public void OnHide() {
        AudioManager.Instance.PlaySFX3D(heartbeatEvent, transform.position);
    }

    public void OnGodown() {
        AudioManager.Instance.PlaySFX3D(godownEvent, transform.position);
    }
    public void OnDeath() {
        AudioManager.Instance.PlaySFX3D(deathEvent, transform.position);
    }
    public void OnLedgeGrab() {
        AudioManager.Instance.PlaySFX3D(ledgegrabEvent, transform.position);
    }


    }