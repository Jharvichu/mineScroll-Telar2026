using UnityEngine;
using StateMachine;

namespace Player.Movement.States{
	[CreateAssetMenu(
	fileName = "SO_GroundState", 
	menuName = "Player/Movement States/Ground State",
	order = 0)]
	public class SO_GroundState : SO_State {
		
		public float HorizontalVelocity;

		[Header("Ground Raycast")]
		public LayerMask GroundLayer;	 
		public float GroundRaycastDistance;
		public float GroundRaycastAmplitude;

		[Header("Ledge Raycast")]
		public LayerMask LedgeLayer;	 
		public float LedgeRaycastDistance;
		public float LedgeRaycastAmplitude;

		public SO_GroundState()
		{
			State = new GroundState(this);
			NodeType = MovementState.Ground;
		}

	}
}

