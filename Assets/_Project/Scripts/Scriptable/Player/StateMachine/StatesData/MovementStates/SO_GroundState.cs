using UnityEngine;
using StateMachine;

namespace Player.States{
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

		public SO_GroundState()
		{
			State = new GroundState(this);
			NodeType = MovementSate.Ground;
		}

	}
}
