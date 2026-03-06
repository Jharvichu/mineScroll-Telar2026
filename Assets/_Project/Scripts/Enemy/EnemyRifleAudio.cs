using UnityEngine;
using FMODUnity;

public class EnemyRifleAudio : MonoBehaviour {
    [SerializeField] EventReference footstepEvent;
    [SerializeField] EventReference reloadEvent;
    [SerializeField] EventReference shootEvent;

    public void OnFootstep() {
        AudioManager.Instance.PlaySFX3D(footstepEvent, transform.position);
    }

    public void OnReload() {
        AudioManager.Instance.PlaySFX3D(reloadEvent, transform.position);
    }
    public void OnShoot() {
        AudioManager.Instance.PlaySFX3D(shootEvent, transform.position);
    }

    }