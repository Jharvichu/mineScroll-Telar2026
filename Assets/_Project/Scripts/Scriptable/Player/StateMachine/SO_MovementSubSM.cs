using UnityEngine;
using StateMachine;

namespace Player {
	
	[CreateAssetMenu(
		fileName = "SO_MovementSubSM", 
		menuName = "Player/Movement Sub-State Machine")]
	public class SO_MovementSubSM : SO_StateMachine {

		[Header("Ground Raycast")]
		public LayerMask GroundLayer;	 
		public float GroundRaycastDistance;
		public float GroundRaycastAmplitude;

		[Header("Debug Options")]
		public bool EnableDebug = false;

        public SO_MovementSubSM(){
			State = new MovementSubSM(this);
			NodeType = PlayerState.Movement;
		}
	}

}
