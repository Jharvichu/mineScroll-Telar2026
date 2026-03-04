using UnityEngine;

namespace Player
{
    public class AnimatorController : MonoBehaviour
    {
        [SerializeField] private Animator _playerAnimator;

        private Rigidbody2D _rigidbody;
        private PlayerController _player;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _player = GetComponent<PlayerController>();
        }

        private void LateUpdate()
        {
            SetVelocity();
            SetClimbing();
            SetCrouching();
            SetGrounded();
        }

        public void SetVelocity()
        {
            float velocity = _rigidbody.linearVelocity.x;
            float absVelocity = Mathf.Abs(velocity);
            _playerAnimator.SetFloat("VelocityX", absVelocity);
        }

        public void SetClimbing()
        {
            _playerAnimator.SetBool("isClimbing", _player.isClimbing);
        }

        public void SetCrouching()
        {
            _playerAnimator.SetBool("isCrouching", _player.isCrouching);
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
