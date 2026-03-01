using UnityEngine;
using StateMachine;

namespace Player.Movement.States{
	[CreateAssetMenu(
	fileName = "SO_GroundState", 
	menuName = "Player/Movement States/Ground State",
	order = 0)]
	public class SO_GroundState : SO_State {
		
		public float HorizontalVelocity;
		public float VerticalVelocity;

		[Header("Ground Raycast")]
		public LayerMask GroundLayer;	 
		public float GroundRaycastDistance;
		public float GroundRaycastAmplitude;

        [Header("Ledge Above Raycasts")]
        public LayerMask LedgeAboveLayer;
        public float LedgeRaycastMiddleDistance;
        public float LedgeMiddleHeight;
        public float LedgeRaycastTopDistance;
        public float LedgeTopHeight;

        [Header("Ledge Below Raycast")]
        public LayerMask LedgeBelowLayer;
        public float LedgeBelowRaycastAmplitude;
        public float LedgeBelowRaycastDistance;

        public SO_GroundState()
		{
			State = new GroundState(this);
			NodeType = MovementState.Ground;
		}

	}
}

