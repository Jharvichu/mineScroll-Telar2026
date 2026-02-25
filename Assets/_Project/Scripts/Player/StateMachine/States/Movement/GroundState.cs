using UnityEngine;
using StateMachine;

namespace Player.States {

	public class GroundState : AState {

		private readonly SO_GroundState _groundData;
		private PlayerController _player;
		private MovementSubSM _movementSM;
		private Rigidbody2D _rb;

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
			base.UpdateState();
		}

		public override void FixedUpdateState() {
			HorizontalMove();
            base.FixedUpdateState();
		}

		public override void ExitState() {
			base.ExitState();
		}

		private void GetInputs()
		{
			_rightInput = Input.GetKey(KeyCode.D);
			_leftInput = Input.GetKey(KeyCode.A);
			_upInput = Input.GetKey(KeyCode.W);
			_downInput = Input.GetKey(KeyCode.S);
		}

		private void HorizontalMove() {

			float horizontalDirection = 0;
			if (_rightInput) horizontalDirection += 1;
			if (_leftInput) horizontalDirection += -1;

			_player.FlipSprite(horizontalDirection);
			_rb.linearVelocityX = horizontalDirection * _groundData.HorizontalVelocity;
		}

	}
}