using UnityEngine;
using Player;

public class HidingBote : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Collider2D ceilingCollider;

    private int _playerLayer;

    private void Awake()
    {
        _playerLayer = LayerMask.NameToLayer("Player");
    }

    private void Start()
    {
        if (ceilingCollider != null) ceilingCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != _playerLayer) return;

        if (collision.TryGetComponent(out PlayerController player))
        {
            if (player.isCrouching && player.isHidden)
            {
                player.isHidden = true;
                SetCeilingActive(true);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer != _playerLayer) return;

        if (collision.TryGetComponent(out PlayerController player))
        {
            if (player.isCrouching && player.isHidden)
            {
                player.isHidden = true;
                SetCeilingActive(true);
            }

        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer != _playerLayer) return;

        if (collision.TryGetComponent(out PlayerController player))
        {
            player.isHidden = false;
            SetCeilingActive(false);
        }
    }

    private void SetCeilingActive(bool state)
    {
        if (ceilingCollider != null)
        {
            ceilingCollider.enabled = state;
        }
    }
}
