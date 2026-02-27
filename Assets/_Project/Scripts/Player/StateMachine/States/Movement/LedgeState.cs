using UnityEngine;
using StateMachine;

namespace Player.Movement.States {

	public class LedgeState : AState {

		private SO_LedgeState _ledgeData;
		private PlayerController _player;
		private Rigidbody2D _rb;
		private MovementSubSM _movementSM;

		private float _timer;

		private RaycastHit2D _wallDetectionHit;
		private bool _isClimbingUp;

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
			_timer = 0f;
			_rb.gravityScale = 0f;
			_rb.linearVelocity = Vector2.zero;
			_downInput = false;
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
			_rb.gravityScale = 1f;
			_isClimbingUp = false;

			base.ExitState();
		}

		private void Checkledge(){
			
			Debug.Log("Timer: " + _timer);
			if(_timer >= _ledgeData.CliffTime && !_isClimbingUp)
			{
				Drop();
				return;
			}

			if (_downInput) 
			{
				Drop();
			}
			
			if (_upInput || _isClimbingUp)
			{
				_isClimbingUp = true;
				Raise();
			}
		}

		private void Drop() {
			Debug.Log("Se solto! ");
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

			if (!_isClimbingUp || _wallDetectionHit) return;

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
            if (!_isClimbingUp || _wallDetectionHit) return;

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