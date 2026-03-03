using UnityEngine;

namespace Player
{
    public class AnimatorController : MonoBehaviour
    {
        [SerializeField] private Animator _playerAnimator;

        private Rigidbody2D _rigidbody;
        private PlayerController _playerController;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _playerController = GetComponent<PlayerController>();
        }

        private void LateUpdate()
        {
            SetVelocity();
            SetGrounded();
        }

        public void SetVelocity()
        {
            float velocity = _rigidbody.linearVelocity.x;
            float absVelocity = Mathf.Abs(velocity);
            _playerAnimator.SetFloat("VelocityX", absVelocity);
        }

        public void SetGrounded()
        {
        }

        public void SetJumping(bool isJumping)
        {

        }

        public void SetHanging(bool isHanging)
        {

        }

        public void TriggerClimbing()
        {

        }

        public void TriggerDropping()
        {

        }
    }

}
