using UnityEngine;
using StateMachine;

namespace Player.Movement.States {

	public class GroundState : AState {

		private readonly SO_GroundState _groundData;
		private PlayerController _player;
		private MovementSubSM _movementSM;
		private Rigidbody2D _rb;

		private RaycastHit2D groundHitLeft, groundHitRight;
		private RaycastHit2D ledgeSideLeft, ledgeSideRight;
		private bool isLedgeDetected;
		private bool _isGrabbing;

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
			_isGrabbing = false;
			base.ExitState();
		}

		private void HorizontalMove() {
			if (_isGrabbing) return;

			float horizontalDirection = 0;
			if (_rightInput) horizontalDirection += 1;
			if (_leftInput) horizontalDirection += -1;

			_player.FlipSprite(horizontalDirection);
			_rb.linearVelocityX = horizontalDirection * _groundData.HorizontalVelocity;
		}

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
				Color.red
			);

			Debug.DrawLine(
				(Vector2)_player.transform.position + Vector2.right * _groundData.GroundRaycastAmplitude,
				(Vector2)_player.transform.position +
				Vector2.right * _groundData.GroundRaycastAmplitude +
				Vector2.down * _groundData.GroundRaycastDistance,
				Color.red
			);
		}

		private void DrawLedgeRaycastDebug()
		{
			if (!_isGrabbing) return;

			Debug.DrawLine(
				(Vector2)_player.transform.position - Vector2.right * _groundData.LedgeRaycastAmplitude,
				(Vector2)_player.transform.position -
				Vector2.right * _groundData.LedgeRaycastAmplitude +
				Vector2.down * _groundData.LedgeRaycastDistance,
				Color.green
			);

			Debug.DrawLine(
				(Vector2)_player.transform.position + Vector2.right * _groundData.LedgeRaycastAmplitude,
				(Vector2)_player.transform.position +
				Vector2.right * _groundData.LedgeRaycastAmplitude +
				Vector2.down * _groundData.LedgeRaycastDistance,
				Color.green
			);
		}

	}
}