using UnityEngine;
using StateMachine;

namespace Player {
	
	[CreateAssetMenu(
		fileName = "SO_MovementSubSM", 
		menuName = "Player/Movement Sub-State Machine")]
	public class SO_MovementSubSM : SO_StateMachine {

		public float ColldownTime;

		public LayerMask OcultarLayer;

		[Header("Ground Raycast")]
		public LayerMask GroundLayer;	 
		public float GroundRaycastDistance;
		public float GroundRaycastAmplitude;

		[Header("Ledge Raycasts")]
		public LayerMask CliffLayer;
		public float CliffRaycastMiddleDistance;
		public float CliffMiddleHeight; 
		public float CliffRaycastTopDistance;
		public float CliffTopHeight;

        [Header("Hidden Raycast")]
        public LayerMask HiddenSpotLayer;
        public float DetectionRaycastDistance;
        public float DetectionRaycastAmplitude;

        [Header("Hiding Spot Crouch Detection")]
        public LayerMask HiddenSpotCrouchLayer;
        public float DetectionRaycastOffSetY;
        public float DetectionRaycastSizeY;

        [Header("Debug Options")]
		public bool EnableDebug = false;

		public SO_MovementSubSM(){
			State = new MovementSubSM(this);
			NodeType = PlayerState.Movement;
		}
	}

}
