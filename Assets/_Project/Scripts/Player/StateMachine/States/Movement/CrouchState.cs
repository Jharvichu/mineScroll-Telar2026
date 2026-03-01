using UnityEngine;
using StateMachine;

namespace Player.Movement.States
{
    public class CrouchState : AState
    {
        private readonly SO_CrouchState _crouchData;
        private PlayerController _player;
        private Rigidbody2D _rb;

        private RaycastHit2D groundHitLeft, groundHitRight;
        private RaycastHit2D ceilingDetectionHit;

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
            DetectCeilingAndGround();
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
            _player.isCrouching = true;
            Vector3 scale = _player.Rigidbody2D.transform.localScale;
            scale.y = 0.5f;
            _player.Rigidbody2D.transform.localScale = scale;
        }

        private void TryExitCrouchState()
        {
            _player.isCrouching = false;
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
            else if(_rightInput)
            {
                if ( !groundHitRight && groundHitLeft ) _rb.linearVelocityX = 0;
                else horizontalDirection += 1;
            }
            else if (_leftInput)
            {
                if ( groundHitRight && !groundHitLeft ) _rb.linearVelocityX = 0;
                else horizontalDirection -= 1;
            }

            _player.FlipSprite(horizontalDirection);
            _rb.linearVelocityX = horizontalDirection * _crouchData.HorizontalVelocity;
        }

        private void DetectCeilingAndGround()
        {
            DetectGround();
            DetectCeiling();
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

        private void DetectCeiling()
        {
            ceilingDetectionHit = Physics2D.BoxCast(
                (Vector2)_player.transform.position + Vector2.up * _crouchData.CeilingBoxOffset,
                _crouchData.CeilingBoxSize, 0f,
                Vector2.up,
                _crouchData.CeilingCheckDistance,
                _crouchData.CeilingLayer
            );

            _player.isCeilingBlocked = ceilingDetectionHit.collider != null;
        }

        private void DrawDetectionDebugLines()
        {
            DrawCeilinglBoxCastDebug();
            DrawGroundRaycastDebug();
        }

        private void DrawCeilinglBoxCastDebug()
        {
            Vector2 origin = (Vector2)_player.transform.position + Vector2.up * _crouchData.CeilingBoxOffset;
            Vector2 end = origin + Vector2.up * _crouchData.CeilingCheckDistance;
            Vector2 half = _crouchData.CeilingBoxSize * 0.5f;

            Debug.DrawLine(end + new Vector2(-half.x, -half.y), end + new Vector2(half.x, -half.y), Color.green);
            Debug.DrawLine(end + new Vector2(half.x, -half.y), end + new Vector2(half.x, half.y), Color.green);
            Debug.DrawLine(end + new Vector2(half.x, half.y), end + new Vector2(-half.x, half.y), Color.green);
            Debug.DrawLine(end + new Vector2(-half.x, half.y), end + new Vector2(-half.x, -half.y), Color.green);
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
    }
}