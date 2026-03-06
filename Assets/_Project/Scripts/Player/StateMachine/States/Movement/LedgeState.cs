using StateMachine;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;
using FMODUnity;

namespace Player.Movement.States {

	public class LedgeState : AState {

		private SO_LedgeState _ledgeData;
		private PlayerController _player;
		private Rigidbody2D _rb;
		private MovementSubSM _movementSM;

		private float _timer;

		private RaycastHit2D _wallDetectionHit;

        public LedgeState(SO_State data) : base(data) {
			_ledgeData = data as SO_LedgeState;
		}

		public override void Init(StateMachineController controller, AStateMachine parent = null) {
			base.Init(controller, parent);
			_player = controller as PlayerController;
			_rb = _player.Rigidbody2D;
			_movementSM = parent as MovementSubSM;
		}

		public override void EnterState() {
			base.EnterState();
			Debug.Log("Enter to ledge state");
			TryEnterLedgeState();

        }

		public override void UpdateState() {
			_timer += Time.deltaTime;
			GetInputs();
			DetectWallAndGround();
			DrawDetectionDebugLines();
			base.UpdateState();
		}

		public override void FixedUpdateState() {
			Checkledge();
			base.FixedUpdateState();
		}

		public override void ExitState() {
			TryExitLedgeState();
            base.ExitState();
		}

        private void TryEnterLedgeState()
        {
            RuntimeManager.PlayOneShot("event:/SFX_PLAYER_Ledge");
            _player.isDropping = false;
            _timer = 0f;
            _rb.gravityScale = 0f;
            _rb.linearVelocity = Vector2.zero;
            _downInput = false;
        }

        private void TryExitLedgeState()
        {
            _rb.gravityScale = 1f;
            _player.isClimbing = false;
        }

        private void Checkledge()
		{
			if(_player.isClimbing || _spaceInput)
            {
				_player.isClimbing = true;
                Raise();
            }
			else if(_timer >= _ledgeData.CliffTime && !_player.isClimbing)
			{
				Drop();
				return;
			}
			else if (_downInput) 
			{
				Drop();
			}
		}

		private void Drop() {
            RuntimeManager.PlayOneShot("event:/SFX_PLAYER_Bajar");
            _player.isDropping = true;
            _movementSM?.ActivateCliffCooldown();
			_parent.ChangeState(MovementState.Air);
		}

		private void Raise()
		{
			if (_wallDetectionHit) 
				_rb.linearVelocity = new Vector2(0, _ledgeData.raiseSpeed);
			else 
				_rb.linearVelocity = new Vector2(_ledgeData.raiseSpeed * _player.FacingDirection, 0);
		}

		private void DetectWallAndGround()
		{
			_wallDetectionHit = Physics2D.BoxCast(
				(Vector2)_player.transform.position + Vector2.up * _ledgeData.WallBoxOffset,
				_ledgeData.WallBoxSize, 0f,
				Vector2.right * _player.FacingDirection,
				_ledgeData.WallBoxDistance,
				_ledgeData.WallLayer
			);

			if (!_player.isClimbing || _wallDetectionHit) return;

			RaycastHit2D groundHitLeft = Physics2D.Raycast(
				(Vector2)_player.transform.position - Vector2.right * _ledgeData.GroundRaycastAmplitude,
				Vector2.down,
				_ledgeData.GroundRaycastDistance,
				_ledgeData.GroundLayer);

			RaycastHit2D groundHitRight = Physics2D.Raycast(
				(Vector2)_player.transform.position + Vector2.right * _ledgeData.GroundRaycastAmplitude,
				Vector2.down,
				_ledgeData.GroundRaycastDistance,
				_ledgeData.GroundLayer);

			if (groundHitLeft && groundHitRight)
			{
				_parent.ChangeState(MovementState.Ground);
			}
		}
		
		private void DrawDetectionDebugLines()
		{
			DrawWallBoxCastDebug();
			DrawGroundRaycastDebug();
		}

		private void DrawWallBoxCastDebug()
		{
			Vector2 origin = (Vector2)_player.transform.position + Vector2.up * _ledgeData.WallBoxOffset;
			Vector2 end = origin + Vector2.right * _player.FacingDirection * _ledgeData.WallBoxDistance;
			Vector2 half = _ledgeData.WallBoxSize * 0.5f;

			Debug.DrawLine(end + new Vector2(-half.x, -half.y), end + new Vector2(half.x, -half.y), Color.magenta);
			Debug.DrawLine(end + new Vector2(half.x, -half.y), end + new Vector2(half.x, half.y), Color.magenta);
			Debug.DrawLine(end + new Vector2(half.x, half.y), end + new Vector2(-half.x, half.y), Color.magenta);
			Debug.DrawLine(end + new Vector2(-half.x, half.y), end + new Vector2(-half.x, -half.y), Color.magenta);
		}

		private void DrawGroundRaycastDebug()
		{
            if (!_player.isClimbing || _wallDetectionHit) return;

            Debug.DrawLine(
				(Vector2)_player.transform.position - Vector2.right * _ledgeData.GroundRaycastAmplitude,
				(Vector2)_player.transform.position -
				Vector2.right * _ledgeData.GroundRaycastAmplitude +
				Vector2.down * _ledgeData.GroundRaycastDistance,
				Color.magenta
			);

			Debug.DrawLine(
				(Vector2)_player.transform.position + Vector2.right * _ledgeData.GroundRaycastAmplitude,
				(Vector2)_player.transform.position +
				Vector2.right * _ledgeData.GroundRaycastAmplitude +
				Vector2.down * _ledgeData.GroundRaycastDistance,
				Color.magenta
			);
		}
	}

}