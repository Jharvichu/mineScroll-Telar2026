using UnityEngine;
using StateMachine;

namespace Player.Movement.States{
	[CreateAssetMenu(
	fileName = "SO_LedgeState", 
	menuName = "Player/Movement States/Ledge State",
	order = 1)]
	public class SO_LedgeState : SO_State {

		public float CliffTime;
		public float raiseSpeed;

		[Header("Wall Boxcast")]
		public LayerMask WallLayer;
		public Vector2 WallBoxSize;
		public float WallBoxOffset;
		public float WallBoxDistance;

		[Header("Ground Raycast")]
		public LayerMask GroundLayer;	 
		public float GroundRaycastDistance;
		public float GroundRaycastAmplitude;

		public SO_LedgeState()
		{
			State = new LedgeState(this);
			NodeType = MovementState.Ledge;
		}

	}
}