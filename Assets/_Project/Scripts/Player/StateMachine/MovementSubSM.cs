using StateMachine;
using UnityEngine;

namespace Player {
	public class MovementSubSM : AStateMachine {
		private SO_MovementSubSM _movementData;
		private PlayerController _player;

		public MovementSubSM(SO_StateMachine data) : base(data) {
			_movementData = data as SO_MovementSubSM;
		}

		public override void Init(StateMachineController controller, AStateMachine parent = null) {
			base.Init(controller, parent);
			_player = controller as PlayerController;
		}

		public override void EnterState() {
			ChangeState(MovementSate.Ground); //Estado inicial
			base.EnterState();
		}

		public override void UpdateState() {
			DrawDebug();
			base.UpdateState();
		}

		public override void FixedUpdateState() {
			base.FixedUpdateState();
            // Funciones en Loop
		}

        public override void ExitState() {
			base.ExitState();
		}

        private void DrawDebug() {
			if (!_movementData.EnableDebug) return;
			DrawGroundLines();
		}

		private void CheckGround() {
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

			if (groundHitLeft || groundHitRight) {
				ChangeState(MovementSate.Ground);
			}
		}

		private void DrawGroundLines() {
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
	}
}