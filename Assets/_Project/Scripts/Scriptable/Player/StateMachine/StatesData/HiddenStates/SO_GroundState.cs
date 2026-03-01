using UnityEngine;
using StateMachine;

namespace Player.Hidden.States{
	[CreateAssetMenu(
	fileName = "SO_GroundState", 
	menuName = "Player/Hidden States/Ground State",
	order = 0)]
	public class SO_GroundState : SO_State {
		
		public float HorizontalVelocity;

        [Header("Ground Raycast")]
        public LayerMask GroundLayer;
        public float GroundRaycastDistance;
        public float GroundRaycastAmplitude;

        [Header("Hiding Spot Detection")]
        public LayerMask HidingSpotLayer;
        public float DetectionRaycastDistance;
        public float DetectionRaycastAmplitude;

        public SO_GroundState()
		{
			State = new GroundState(this);
			NodeType = HiddenState.Ground;
		}

	}
}