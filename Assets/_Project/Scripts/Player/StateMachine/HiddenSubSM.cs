using System;
using StateMachine;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace Player {

	public class HiddenSubSM : AStateMachine
	{
		private SO_HiddenSubSM _hiddenData;
		private PlayerController _player;

		private int _originalSortingOrder;
        private bool _shouldCrouch = false;

		public HiddenSubSM(SO_StateMachine data) : base(data)
		{
			_hiddenData = data as SO_HiddenSubSM;
		}

		public override void Init(StateMachineController controller, AStateMachine parent = null)
		{
			base.Init(controller, parent);
			_player = controller as PlayerController;
		}

		public override void EnterState()
		{
            Debug.Log("Enter to Hidden State");
			HidePlayerBehindObstacle(_player.CurrentHidingSpotSprite);
            ChangeState(HiddenState.Ground);
			base.EnterState();
		}
		public override void UpdateState()
		{
            DrawDebug();
			CheckMovementMode();
			base.UpdateState();
		}

		public override void FixedUpdateState()
		{
			base.FixedUpdateState();
		}

        public override void ExitState()
        {
			ShowPlayerInFront();
            base.ExitState();
        }

		private void CheckMovementMode()
		{
			if (_modeInput && _player.isHidden)
			{
				_player.isHidden = false;
                _parent.ChangeState(PlayerState.Movement);
			}
		}

        private void HidePlayerBehindObstacle(SpriteRenderer obstacleSprite)
        {
            _originalSortingOrder = _player.SpriteRenderer.sortingOrder;
            _player.SpriteRenderer.sortingOrder = obstacleSprite.sortingOrder - 1;
        }

        private void ShowPlayerInFront()
        {
			Debug.Log("Salio del escondite");
            _player.SpriteRenderer.sortingOrder = _originalSortingOrder;
        }

        private void DrawDebug()
		{
			if (!_hiddenData.EnableDebug) return;
			DrawGroundLines();
		}

		private void CheckGround()
		{
			RaycastHit2D groundHitLeft = Physics2D.Raycast(
				(Vector2)_player.transform.position - Vector2.right * _hiddenData.GroundRaycastAmplitude,
				Vector2.down,
				_hiddenData.GroundRaycastDistance,
				_hiddenData.GroundLayer);

			RaycastHit2D groundHitRight = Physics2D.Raycast(
				(Vector2)_player.transform.position + Vector2.right * _hiddenData.GroundRaycastAmplitude,
				Vector2.down,
				_hiddenData.GroundRaycastDistance,
				_hiddenData.GroundLayer);

			if ((groundHitLeft || groundHitRight) && _shouldCrouch)
			{
				ChangeState(HiddenState.Crouch);
			}
            else if ((groundHitLeft || groundHitRight) && !_shouldCrouch)
            {
                ChangeState(HiddenState.Ground);
            }
		}

		private void DrawGroundLines()
		{
			//Left Line
			Debug.DrawLine(
				(Vector2)_player.transform.position - Vector2.right * _hiddenData.GroundRaycastAmplitude,
				(Vector2)_player.transform.position -
				Vector2.right * _hiddenData.GroundRaycastAmplitude +
				Vector2.down * _hiddenData.GroundRaycastDistance,
				Color.magenta
			);

			//RightLine
			Debug.DrawLine(
				(Vector2)_player.transform.position + Vector2.right * _hiddenData.GroundRaycastAmplitude,
				(Vector2)_player.transform.position +
				Vector2.right * _hiddenData.GroundRaycastAmplitude +
				Vector2.down * _hiddenData.GroundRaycastDistance,
				Color.magenta
			);
		}

        private void GetCrouchInput()
        {
            if (_modeInput)
            {
                _shouldCrouch = !_shouldCrouch;
            }
        }

	}
}