using UnityEngine;
using StateMachine;

namespace Player.Movement.States
{
    public class CrouchState : AState
    {
        private readonly SO_CrouchState _crouchData;
        private PlayerController _player;
        private Rigidbody2D _rb;
        private Vector2 _originalColliderSize, _originalColliderOffset;

        private RaycastHit2D groundHitLeft, groundHitRight;
        private RaycastHit2D ceilingDetectionHit;
        private CapsuleCollider2D _capsuleCollider;

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
            _capsuleCollider = (CapsuleCollider2D)_player.Collider2D;
            _originalColliderSize = _capsuleCollider.size;
            _originalColliderOffset = _capsuleCollider.offset;
            _capsuleCollider.size = _crouchData.ColliderBoxSize;
            _capsuleCollider.offset = _crouchData.ColliderBoxOffset;
        }

        private void TryExitCrouchState()
        {
            _player.isCrouching = false;
            _capsuleCollider = (CapsuleCollider2D)_player.Collider2D;
            _capsuleCollider.size   = _originalColliderSize;
            _capsuleCollider.offset = _originalColliderOffset;
        }

        private void HorizontalMove()
        {
            float horizontalDirection = 0;

            if(_rightInput)
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

            Vector2 direction = Vector2.up;
            float distance = _crouchData.CeilingCheckDistance;
            Vector2 size = _crouchData.CeilingBoxSize;
            Vector2 half = size * 0.5f;

            Vector2 end = origin + direction * distance;

            DrawRectangle(origin, half, Color.red);

            DrawRectangle(end, half, Color.green);
        }

        private void DrawRectangle(Vector2 position, Vector2 half, Color color)
        {
            Debug.DrawLine(position + new Vector2(-half.x, -half.y), position + new Vector2(half.x, -half.y), color);
            Debug.DrawLine(position + new Vector2(half.x, -half.y), position + new Vector2(half.x, half.y), color);
            Debug.DrawLine(position + new Vector2(half.x, half.y), position + new Vector2(-half.x, half.y), color);
            Debug.DrawLine(position + new Vector2(-half.x, half.y), position + new Vector2(-half.x, -half.y), color);
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