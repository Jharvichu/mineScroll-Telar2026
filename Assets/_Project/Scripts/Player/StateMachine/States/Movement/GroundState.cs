using UnityEngine;
using StateMachine;

namespace Player.Movement.States {

	public class GroundState : AState {

		private readonly SO_GroundState _groundData;
		private PlayerController _player;
		private MovementSubSM _movementSM;
		private Rigidbody2D _rb;

		private RaycastHit2D groundHitLeft, groundHitRight;
		private RaycastHit2D ledgeBelowLeft, ledgeBelowRight;
		private RaycastHit2D ledgeHitMiddle, ledgeHitTop;

        private bool isLedgeAboveDetected, isLedgeBelowDetected;

		public GroundState(SO_State data) : base(data) {
			_groundData = _stateData as SO_GroundState;
		}

		public override void Init(StateMachineController controller, AStateMachine parent = null) {
			base.Init(controller, parent);
			_player = controller as PlayerController;
			_movementSM = parent as MovementSubSM;
			_rb = _player.Rigidbody2D;
		}

		public override void EnterState() {
			base.EnterState();
			Debug.Log("Enter To Ground State");
		}

		public override void UpdateState() {
            GetInputs();
			DetectLedgeAndGround();
			DrawDetectionDebugLines();
			base.UpdateState();
		}

		public override void FixedUpdateState() {
			HorizontalMove();
			Grab();
            base.FixedUpdateState();
		}

		public override void ExitState() {
			base.ExitState();
		}

		private void HorizontalMove() {
			if (_player.isClimbing || _player.isDroppingToLedge) return;

            float horizontalDirection = 0;

			if (_movementSM.isHidding)
			{
                float directionToSpot = _player.CurrentHidingSpotCollider.transform.position.x - _player.transform.position.x;
                horizontalDirection = Mathf.Sign(directionToSpot) / 3;
            }
			else if (_rightInput)
			{
                if (!groundHitRight && groundHitLeft)
				{
                    _rb.linearVelocityX = 0;
                    _player.FlipSprite(+1);
                }
                else horizontalDirection += 1;
			}
            else if (_leftInput)
            {
                if (groundHitRight && !groundHitLeft)
				{
                    _rb.linearVelocityX = 0;
                    _player.FlipSprite(-1);
                }
                else horizontalDirection -= 1;
            }

            _player.FlipSprite(horizontalDirection);
			_rb.linearVelocityX = horizontalDirection * _groundData.HorizontalVelocity;
		}

		private void Grab()
		{
			if ( (_upInput && isLedgeAboveDetected) || _player.isClimbing)
			{
				_player.isClimbing = true;
                _rb.linearVelocityY = _groundData.VerticalVelocity;
            }
			else if ( (_downInput && isLedgeBelowDetected) || _player.isDroppingToLedge)
			{
                _player.isDroppingToLedge = true;
                float direction = !groundHitRight ? 1 : !groundHitLeft ? -1 : 0;
                _rb.linearVelocityX = direction * _groundData.HorizontalVelocity * 0.5f;

				if (!isLedgeBelowDetected && ledgeBelowLeft) 
				{
                    Debug.Log("LLego aqui, deberia cambiar hacia la izquierda");
                    _player.FlipSprite(-1);
                    _player.isDroppingToLedge = false;
                    _player.Rigidbody2D.linearVelocity = new Vector2(0, -1 * _groundData.HorizontalVelocity * 0.5f);
                }
                if (!isLedgeBelowDetected && ledgeBelowRight)
				{
                    Debug.Log("LLego aqui, deberia cambiar hacia la derecha");
                    _player.FlipSprite(+1);
                    _player.isDroppingToLedge = false;
                    _player.Rigidbody2D.linearVelocity = new Vector2(0, -1 * _groundData.HorizontalVelocity * 0.5f);
                }
            }
        }
        /*
		private void Grab()
		{
			if (_downInput && isLedgeDetected)
			{
				_isGrabbing = true;
				_movementSM?.ActivateGrabbingCooldown();
			}

			if (!_isGrabbing) return;

			if (isLedgeDetected)
			{
				float direction = !groundHitRight ? 1 : !groundHitLeft ? -1 : 0;
				_rb.linearVelocityX = direction * _groundData.HorizontalVelocity * 0.5f;
			}
			else
			{
				if (ledgeSideLeft) _player.FlipSprite(-1);
				if (ledgeSideRight) _player.FlipSprite(+1);
				_player.Rigidbody2D.linearVelocity = new Vector2(0, -1 * _groundData.HorizontalVelocity * 0.5f);
			}
		}
		*/
        /*
		private void DetectLedgeAndGround()
		{
			groundHitLeft = Physics2D.Raycast(
				(Vector2)_player.transform.position - Vector2.right * _groundData.GroundRaycastAmplitude,
				Vector2.down,
				_groundData.GroundRaycastDistance,
				_groundData.GroundLayer);

			groundHitRight = Physics2D.Raycast(
				(Vector2)_player.transform.position + Vector2.right * _groundData.GroundRaycastAmplitude,
				Vector2.down,
				_groundData.GroundRaycastDistance,
				_groundData.GroundLayer);

			isLedgeDetected = groundHitLeft ^ groundHitRight;

			if (!_isGrabbing) return;

			ledgeSideLeft = Physics2D.Raycast(
				(Vector2)_player.transform.position - Vector2.right * _groundData.LedgeRaycastAmplitude,
				Vector2.down,
				_groundData.LedgeRaycastDistance,
				_groundData.LedgeLayer);

			ledgeSideRight = Physics2D.Raycast(
				(Vector2)_player.transform.position + Vector2.right * _groundData.LedgeRaycastAmplitude,
				Vector2.down,
				_groundData.LedgeRaycastDistance,
				_groundData.LedgeLayer);
		}
		*/

        private void DetectLedgeAndGround()
		{
			DetectGround();
			DetectLedgeBelow();
			DetectLedgeAbove();
        }


        private void DetectGround()
		{
            groundHitLeft = Physics2D.Raycast(
                (Vector2)_player.transform.position - Vector2.right * _groundData.GroundRaycastAmplitude,
                Vector2.down,
                _groundData.GroundRaycastDistance,
                _groundData.GroundLayer);

            groundHitRight = Physics2D.Raycast(
                (Vector2)_player.transform.position + Vector2.right * _groundData.GroundRaycastAmplitude,
                Vector2.down,
                _groundData.GroundRaycastDistance,
                _groundData.GroundLayer);

            isLedgeBelowDetected = groundHitLeft ^ groundHitRight;
        }

		private void DetectLedgeBelow()
		{
            ledgeBelowLeft = Physics2D.Raycast(
				(Vector2)_player.transform.position - Vector2.right * _groundData.LedgeBelowRaycastAmplitude,
				Vector2.down,
				_groundData.LedgeBelowRaycastDistance,
				_groundData.LedgeBelowLayer);

            ledgeBelowRight = Physics2D.Raycast(
                (Vector2)_player.transform.position + Vector2.right * _groundData.LedgeBelowRaycastAmplitude,
                Vector2.down,
                _groundData.LedgeBelowRaycastDistance,
                _groundData.LedgeBelowLayer);
        }

		private void DetectLedgeAbove()
		{
            ledgeHitMiddle = Physics2D.Raycast(
                (Vector2)_player.transform.position + Vector2.up * _groundData.LedgeRaycastMiddleDistance,
                Vector2.right * _player.FacingDirection,
                _groundData.LedgeMiddleHeight,
                _groundData.LedgeAboveLayer
            );

            ledgeHitTop = Physics2D.Raycast(
                (Vector2)_player.transform.position + Vector2.up * _groundData.LedgeRaycastTopDistance,
                Vector2.right * _player.FacingDirection,
				_groundData.LedgeTopHeight,
                _groundData.LedgeAboveLayer
            );

            isLedgeAboveDetected = (ledgeHitMiddle.collider != null) && (ledgeHitTop.collider == null);
        }

		private void DrawDetectionDebugLines()
		{
			DrawGroundRaycastDebug();
			DrawLedgeRaycastDebug();
		}

		private void DrawGroundRaycastDebug()
		{
			Debug.DrawLine(
				(Vector2)_player.transform.position - Vector2.right * _groundData.GroundRaycastAmplitude,
				(Vector2)_player.transform.position -
				Vector2.right * _groundData.GroundRaycastAmplitude +
				Vector2.down * _groundData.GroundRaycastDistance,
				Color.yellow
			);

			Debug.DrawLine(
				(Vector2)_player.transform.position + Vector2.right * _groundData.GroundRaycastAmplitude,
				(Vector2)_player.transform.position +
				Vector2.right * _groundData.GroundRaycastAmplitude +
				Vector2.down * _groundData.GroundRaycastDistance,
				Color.yellow
			);
		}

		private void DrawLedgeRaycastDebug()
		{
			// Ledge Below
			Debug.DrawLine(
				(Vector2)_player.transform.position - Vector2.right * _groundData.LedgeBelowRaycastAmplitude,
				(Vector2)_player.transform.position -
				Vector2.right * _groundData.LedgeBelowRaycastAmplitude +
				Vector2.down * _groundData.LedgeBelowRaycastDistance,
				Color.red
			);

			Debug.DrawLine(
				(Vector2)_player.transform.position + Vector2.right * _groundData.LedgeBelowRaycastAmplitude,
				(Vector2)_player.transform.position +
				Vector2.right * _groundData.LedgeBelowRaycastAmplitude +
				Vector2.down * _groundData.LedgeBelowRaycastDistance,
				Color.red
			);

            // Ledge Above
            Debug.DrawLine(
                (Vector2)_player.transform.position + Vector2.up * _groundData.LedgeRaycastMiddleDistance,
                (Vector2)_player.transform.position +
                Vector2.up * _groundData.LedgeRaycastMiddleDistance +
                Vector2.right * _groundData.LedgeMiddleHeight * _player.FacingDirection,
                Color.yellow
            );

            Debug.DrawLine(
                (Vector2)_player.transform.position + Vector2.up * _groundData.LedgeRaycastTopDistance,
                (Vector2)_player.transform.position +
                Vector2.up * _groundData.LedgeRaycastTopDistance +
                Vector2.right * _groundData.LedgeTopHeight * _player.FacingDirection,
                Color.red
            );
        }

	}
}