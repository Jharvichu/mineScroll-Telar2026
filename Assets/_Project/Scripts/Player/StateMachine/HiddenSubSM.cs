using StateMachine;
using System;
using UnityEngine;
#if UNITY_EDITOR
using static UnityEditor.Experimental.GraphView.GraphView;
#endif

using static UnityEngine.RuleTile.TilingRuleOutput;

namespace Player {

	public class HiddenSubSM : AStateMachine
	{
		private SO_HiddenSubSM _hiddenData;
		private PlayerController _player;

		private int _originalSortingOrder;
        private string _originalSortingLayerName; 

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
			HidePlayerBehindObstacle(_player.CurrentHidingSpotCollider);
            AudioManager.Instance.SetBGMParameter("escondido", 1f);
            ChangeState(HiddenState.Ground);
			base.EnterState();
		}
		public override void UpdateState()
		{
            DrawDebug();
			TryExitHiddenState();
            base.UpdateState();
		}

		public override void FixedUpdateState()
		{
			CheckGround();
            base.FixedUpdateState();
		}

        public override void ExitState()
        {
			ShowPlayerInFront();
            AudioManager.Instance.SetBGMParameter("escondido", 0f);
            base.ExitState();
        }

		private void TryExitHiddenState()
        {
            RaycastHit2D hiddenEntryHit = Physics2D.Raycast(
                (Vector2)_player.transform.position + Vector2.up * _hiddenData.DetectionRaycastOffSetY,
                Vector2.up,
                _hiddenData.DetectionRaycastSizeY,
                _hiddenData.HidingSpotCrouchLayer);

            if (_downInput && _player.isHidden && !hiddenEntryHit)
            {
				_player.isHidden = false;
                _parent.ChangeState(PlayerState.Movement);
			}
			else if (!_player.isHidden  && !_player.isCrouching)
			{
                _player.isHidden = false;
                _parent.ChangeState(PlayerState.Movement);
            }
		}

        private void HidePlayerBehindObstacle(Collider2D obstacleCollision)
        {
            if (obstacleCollision == null) return;

            SpriteRenderer obstacleSprite = obstacleCollision.GetComponent<SpriteRenderer>();
            if (obstacleSprite == null) obstacleSprite = obstacleCollision.GetComponentInChildren<SpriteRenderer>();
            if (obstacleSprite == null) obstacleSprite = obstacleCollision.GetComponentInParent<SpriteRenderer>();

            if (obstacleSprite != null)
            {
                _originalSortingOrder = _player.SpriteRenderer.sortingOrder;
                _originalSortingLayerName = _player.SpriteRenderer.sortingLayerName; 

                _player.SpriteRenderer.sortingLayerName = obstacleSprite.sortingLayerName;
                
                
                _player.SpriteRenderer.sortingOrder = obstacleSprite.sortingOrder + 1; 
            }
        }

        private void ShowPlayerInFront()
        {
            
            _player.SpriteRenderer.sortingOrder = _originalSortingOrder;
            _player.SpriteRenderer.sortingLayerName = _originalSortingLayerName; 
        }

        private void DrawDebug()
		{
			if (!_hiddenData.EnableDebug) return;
			DrawGroundLines();
			DrawDetectionLine();

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

            if ( groundHitLeft && groundHitRight && (_ctrlInput || _player.isCeilingBlocked) )
            {
                ChangeState(HiddenState.Crouch);
            }
            else if (groundHitLeft || groundHitRight)
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
				Color.cyan
			);

			//RightLine
			Debug.DrawLine(
				(Vector2)_player.transform.position + Vector2.right * _hiddenData.GroundRaycastAmplitude,
				(Vector2)_player.transform.position +
				Vector2.right * _hiddenData.GroundRaycastAmplitude +
				Vector2.down * _hiddenData.GroundRaycastDistance,
				Color.cyan
			);
		}

		private void DrawDetectionLine()
		{
            Debug.DrawLine(
                (Vector2)_player.transform.position + Vector2.up * _hiddenData.DetectionRaycastOffSetY,
                (Vector2)_player.transform.position +
                Vector2.up * _hiddenData.DetectionRaycastOffSetY * _hiddenData.DetectionRaycastSizeY,
                Color.yellow
            );
        }
	}
}