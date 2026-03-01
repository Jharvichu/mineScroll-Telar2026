using UnityEngine;
using StateMachine;

namespace Player.Hidden.States
{
    public class CrouchState : AState
    {
        private readonly SO_CrouchState _crouchData;
        private PlayerController _player;
        private Rigidbody2D _rb;

        private RaycastHit2D groundHitLeft, groundHitRight;
        private RaycastHit2D hidingSpotHitLeft, hidingSpotHitRight;

        public CrouchState(SO_State data) : base(data)
        {
            _crouchData = _stateData as SO_CrouchState;
        }

        public override void Init(StateMachineController controller, AStateMachine parent = null)
        {
            base.Init(controller, parent);
            _player = controller as PlayerController;
            _rb = _player.Rigidbody2D;
        }

        public override void EnterState()
        {
            Debug.Log("Enter to crouch State");
            TryEnterCrouchState();
        }

        public override void UpdateState()
        {
            GetInputs();
            DetectHidingAndGround();
            DrawDetectionDebugLines();
            base.UpdateState();
        }

        public override void FixedUpdateState()
        {
            HorizontalMove();
            base.FixedUpdateState();
        }

        public override void ExitState()
        {
            TryExitCrouchState();
            base.ExitState();
        }

        private void TryEnterCrouchState()
        {
            Vector3 scale = _player.Rigidbody2D.transform.localScale;
            scale.y = 0.5f;
            _player.Rigidbody2D.transform.localScale = scale;
        }

        private void TryExitCrouchState()
        {
            Vector3 scale = _player.Rigidbody2D.transform.localScale;
            scale.y = 1f;
            _player.Rigidbody2D.transform.localScale = scale;
        }

        private void HorizontalMove()
        {
            float horizontalDirection = 0;

            if (_player.CurrentHidingSpotCollider.gameObject.layer == LayerMask.NameToLayer("HiddenSpotStatic"))
            {
                horizontalDirection = 0;
            }
            else if (_rightInput)
            {
                if ((!groundHitRight && groundHitLeft) || (!hidingSpotHitRight && hidingSpotHitLeft)) _rb.linearVelocityX = 0;
                else horizontalDirection += 1;
            }
            else if (_leftInput)
            {
                if ((groundHitRight && !groundHitLeft) || (hidingSpotHitRight && !hidingSpotHitLeft)) _rb.linearVelocityX = 0;
                else horizontalDirection -= 1;
            }

            _player.FlipSprite(horizontalDirection);
            _rb.linearVelocityX = horizontalDirection * _crouchData.HorizontalVelocity;
        }

        private void DetectHidingAndGround()
        {
            DetectGround();
            DetectHidingSpots();
        }

        private void DetectGround()
        {
            groundHitLeft = Physics2D.Raycast(
                (Vector2)_player.transform.position - Vector2.right * _crouchData.GroundRaycastAmplitude,
                Vector2.down,
                _crouchData.GroundRaycastDistance,
                _crouchData.GroundLayer);

            groundHitRight = Physics2D.Raycast(
                (Vector2)_player.transform.position + Vector2.right * _crouchData.GroundRaycastAmplitude,
                Vector2.down,
                _crouchData.GroundRaycastDistance,
                _crouchData.GroundLayer);
        }

        private void DetectHidingSpots()
        {
            hidingSpotHitLeft = Physics2D.Raycast(
                (Vector2)_player.transform.position - Vector2.right * _crouchData.DetectionRaycastAmplitude,
                Vector2.up,
                _crouchData.DetectionRaycastDistance,
                _crouchData.HidingSpotLayer);

            hidingSpotHitRight = Physics2D.Raycast(
                (Vector2)_player.transform.position + Vector2.right * _crouchData.DetectionRaycastAmplitude,
                Vector2.up,
                _crouchData.DetectionRaycastDistance,
                _crouchData.HidingSpotLayer);
        }

        private void DrawDetectionDebugLines()
        {
            DrawHidingSpotRaycastsDebug();
            DrawGroundRaycastDebug();
        }

        private void DrawGroundRaycastDebug()
        {
            Debug.DrawLine(
                (Vector2)_player.transform.position - Vector2.right * _crouchData.GroundRaycastAmplitude,
                (Vector2)_player.transform.position -
                Vector2.right * _crouchData.GroundRaycastAmplitude +
                Vector2.down * _crouchData.GroundRaycastDistance,
                Color.green
            );

            Debug.DrawLine(
                (Vector2)_player.transform.position + Vector2.right * _crouchData.GroundRaycastAmplitude,
                (Vector2)_player.transform.position +
                Vector2.right * _crouchData.GroundRaycastAmplitude +
                Vector2.down * _crouchData.GroundRaycastDistance,
                Color.green
            );
        }

        private void DrawHidingSpotRaycastsDebug()
        {
            Debug.DrawLine(
                (Vector2)_player.transform.position - Vector2.right * _crouchData.DetectionRaycastAmplitude,
                (Vector2)_player.transform.position -
                Vector2.right * _crouchData.DetectionRaycastAmplitude +
                Vector2.up * _crouchData.DetectionRaycastDistance,
                Color.yellow
            );

            Debug.DrawLine(
                (Vector2)_player.transform.position + Vector2.right * _crouchData.DetectionRaycastAmplitude,
                (Vector2)_player.transform.position +
                Vector2.right * _crouchData.DetectionRaycastAmplitude +
                Vector2.up * _crouchData.DetectionRaycastDistance,
                Color.yellow
            );
        }
    }
}