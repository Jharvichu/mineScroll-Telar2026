using System;
using StateMachine;
using UnityEngine;

namespace Player {

	public class MovementSubSM : AStateMachine
	{
		private SO_MovementSubSM _movementData;
		private PlayerController _player;

		public float _cliffCooldownTimer = 0f;
		public float _grabCooldownTimer = 0f;

		public bool modoLedge = false;			// Importante cambiar nombre o ver otra forma
		public bool isHidding = false;

		public MovementSubSM(SO_StateMachine data) : base(data)
		{
			_movementData = data as SO_MovementSubSM;
		}

		public override void Init(StateMachineController controller, AStateMachine parent = null)
		{
			base.Init(controller, parent);
			_player = controller as PlayerController;
		}

		public override void EnterState()
		{
			Debug.Log("Enter to Movement State");
			ChangeState(MovementState.Ground);
			base.EnterState();
		}
		
		public override void UpdateState()
		{
			DrawDebug();
			TryEnterHiddenState();
            if (_cliffCooldownTimer > 0f) _cliffCooldownTimer -= Time.deltaTime;
			if (_grabCooldownTimer > 0f) _grabCooldownTimer -= Time.deltaTime;
			base.UpdateState();
		}

		public override void FixedUpdateState()
		{
			CheckCliff();
			CheckGround();
			base.FixedUpdateState();
		}

		public override void ExitState()
        {
            base.ExitState();
        }

		private void DrawDebug()
		{
			if (!_movementData.EnableDebug) return;
			DrawGroundLines();
			DrawCliffLines();
			DrawDetectionLines();

        }

        private void TryEnterHiddenState()
        {
            if (GetCurrentState() as MovementState? != MovementState.Ground) return;
            if (_player.CurrentHidingSpotCollider == null) return;

            RaycastHit2D hiddenSpotHitLeft = Physics2D.Raycast(
                (Vector2)_player.transform.position - Vector2.right * _movementData.GroundRaycastAmplitude,
                Vector2.down,
                _movementData.GroundRaycastDistance,
                _movementData.HiddenSpotLayer);

            RaycastHit2D hiddenSpotHitRight = Physics2D.Raycast(
                (Vector2)_player.transform.position + Vector2.right * _movementData.GroundRaycastAmplitude,
                Vector2.down,
                _movementData.GroundRaycastDistance,
                _movementData.HiddenSpotLayer);

            if (( _modeInput && _player.canHide && (hiddenSpotHitLeft || hiddenSpotHitRight) ) || isHidding)
            {
				isHidding = true;

                if (hiddenSpotHitLeft && hiddenSpotHitRight)
				{
                    isHidding = false;
                    _player.isHidden = true;
                    _parent.ChangeState(PlayerState.Hidden);
                }
            }
        }

        private void CheckGround()
		{
			if (GetCurrentState() as MovementState? == MovementState.Ledge) return;

			RaycastHit2D groundHitLeft = Physics2D.Raycast(
				(Vector2)_player.transform.position - Vector2.right * _movementData.GroundRaycastAmplitude,
				Vector2.down,
				_movementData.GroundRaycastDistance,
				_movementData.GroundLayer);

			RaycastHit2D groundHitRight = Physics2D.Raycast(
				(Vector2)_player.transform.position + Vector2.right * _movementData.GroundRaycastAmplitude,
				Vector2.down,
				_movementData.GroundRaycastDistance,
				_movementData.GroundLayer);

			if (groundHitLeft && groundHitRight && _ctrlInput)
			{
                ChangeState(MovementState.Crouch);
            }
			else if ( groundHitLeft || groundHitRight || _grabCooldownTimer > 0 )
			{
				ChangeState(MovementState.Ground);
			}
			else
			{
				ChangeState(MovementState.Air);
			}
		}



		private void CheckCliff()
		{
			if (_cliffCooldownTimer > 0f) return;

			RaycastHit2D cliffHitMiddle = Physics2D.Raycast(
				(Vector2)_player.transform.position + Vector2.up * _movementData.CliffRaycastMiddleDistance,
				Vector2.right * _player.FacingDirection,
				_movementData.CliffMiddleHeight,
				_movementData.CliffLayer
			);

			RaycastHit2D cliffHitTop = Physics2D.Raycast(
				(Vector2)_player.transform.position + Vector2.up * _movementData.CliffRaycastTopDistance,
				Vector2.right * _player.FacingDirection,
				_movementData.CliffTopHeight,
				_movementData.CliffLayer
			);

			if (cliffHitMiddle && !cliffHitTop && (_player.Rigidbody2D.linearVelocityY < 0 || _upInput))
			{
				ChangeState(MovementState.Ledge);
			}
		}

		public void ActivateCliffCooldown()
		{
			_cliffCooldownTimer = _movementData.ColldownTime;
		}
		
		public void ActivateGrabbingCooldown()
        {
			_grabCooldownTimer = _movementData.ColldownTime;
        }

		private void DrawGroundLines()
		{
			//Left Line
			Debug.DrawLine(
				(Vector2)_player.transform.position - Vector2.right * _movementData.GroundRaycastAmplitude,
				(Vector2)_player.transform.position -
				Vector2.right * _movementData.GroundRaycastAmplitude +
				Vector2.down * _movementData.GroundRaycastDistance,
				Color.cyan
			);

			//RightLine
			Debug.DrawLine(
				(Vector2)_player.transform.position + Vector2.right * _movementData.GroundRaycastAmplitude,
				(Vector2)_player.transform.position +
				Vector2.right * _movementData.GroundRaycastAmplitude +
				Vector2.down * _movementData.GroundRaycastDistance,
				Color.cyan
			);
		}

		private void DrawCliffLines(){
			//Middle Line
			Debug.DrawLine(
				(Vector2)_player.transform.position + Vector2.up * _movementData.CliffRaycastMiddleDistance,
				(Vector2)_player.transform.position +
				Vector2.up * _movementData.CliffRaycastMiddleDistance +
				Vector2.right * _movementData.CliffMiddleHeight * _player.FacingDirection,
				Color.cyan
			);

			//HeadLine
			Debug.DrawLine(
				(Vector2)_player.transform.position + Vector2.up * _movementData.CliffRaycastTopDistance,
				(Vector2)_player.transform.position +
				Vector2.up * _movementData.CliffRaycastTopDistance +
				Vector2.right * _movementData.CliffTopHeight * _player.FacingDirection,
				Color.cyan
			);
		}

        private void DrawDetectionLines()
        {
            Debug.DrawLine(
                (Vector2)_player.transform.position - Vector2.right * _movementData.DetectionRaycastAmplitude,
                (Vector2)_player.transform.position -
                Vector2.right * _movementData.DetectionRaycastAmplitude +
                Vector2.up * _movementData.DetectionRaycastDistance,
                Color.cyan
            );

            Debug.DrawLine(
                (Vector2)_player.transform.position + Vector2.right * _movementData.DetectionRaycastAmplitude,
                (Vector2)_player.transform.position +
                Vector2.right * _movementData.DetectionRaycastAmplitude +
                Vector2.up * _movementData.DetectionRaycastDistance,
                Color.cyan
            );
        }
    }
}