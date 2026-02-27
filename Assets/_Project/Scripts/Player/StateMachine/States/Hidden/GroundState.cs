using UnityEngine;
using StateMachine;

namespace Player.Hidden.States {

	public class GroundState : AState {

		private readonly SO_GroundState _groundData;
		private PlayerController _player;
		private Rigidbody2D _rb;

		private RaycastHit2D groundHitLeft, groundHitRight;
		private RaycastHit2D ledgeSideLeft, ledgeSideRight;

		public GroundState(SO_State data) : base(data) {
			_groundData = _stateData as SO_GroundState;
		}

		public override void Init(StateMachineController controller, AStateMachine parent = null) {
			base.Init(controller, parent);
			_player = controller as PlayerController;
			_rb = _player.Rigidbody2D;
		}

		public override void EnterState() {
			base.EnterState();
			Debug.Log("Enter To Ground State");
		}

		public override void UpdateState() {
            GetInputs();
            DetectObstacule();
            DrawGroundRaycastDebug();
            base.UpdateState();
		}

		public override void FixedUpdateState() {
			HorizontalMove();
            base.FixedUpdateState();
		}

		public override void ExitState() {
			base.ExitState();
		}

		private void HorizontalMove() {
			float horizontalDirection = 0;

            if (_rightInput)
            {
                if (!groundHitRight && groundHitLeft) return;
                horizontalDirection += 1;
            }
            else if (_leftInput)
            {
                if (groundHitRight && !groundHitLeft) return;
                horizontalDirection -= 1;
            }

			_player.FlipSprite(horizontalDirection);
			_rb.linearVelocityX = horizontalDirection * _groundData.HorizontalVelocity;
		}

        private void DetectObstacule()
        {
            groundHitLeft = Physics2D.Raycast(
                (Vector2)_player.transform.position - Vector2.right * _groundData.DetectionRaycastAmplitude,
                Vector2.up,
                _groundData.DetectionRaycastDistance,
                _groundData.HidingSpotLayer);

            groundHitRight = Physics2D.Raycast(
                (Vector2)_player.transform.position + Vector2.right * _groundData.DetectionRaycastAmplitude,
                Vector2.up,
                _groundData.DetectionRaycastDistance,
                _groundData.HidingSpotLayer);
        }

        private void DrawGroundRaycastDebug()
        {
            Debug.DrawLine(
                (Vector2)_player.transform.position - Vector2.right * _groundData.DetectionRaycastAmplitude,
                (Vector2)_player.transform.position -
                Vector2.right * _groundData.DetectionRaycastAmplitude +
                Vector2.up * _groundData.DetectionRaycastDistance,
                Color.yellow
            );

            Debug.DrawLine(
                (Vector2)_player.transform.position + Vector2.right * _groundData.DetectionRaycastAmplitude,
                (Vector2)_player.transform.position +
                Vector2.right * _groundData.DetectionRaycastAmplitude +
                Vector2.up * _groundData.DetectionRaycastDistance,
                Color.yellow
            );
        }

    }
}