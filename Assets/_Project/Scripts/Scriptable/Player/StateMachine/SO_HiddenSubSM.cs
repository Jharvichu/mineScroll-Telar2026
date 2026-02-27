using UnityEngine;
using StateMachine;

namespace Player {
	
	[CreateAssetMenu(
		fileName = "SO_HiddenSubSM", 
		menuName = "Player/Hidden Sub-State Machine")]
	public class SO_HiddenSubSM : SO_StateMachine {

		[Header("Ground Raycast")]
		public LayerMask GroundLayer;	 
		public float GroundRaycastDistance;
		public float GroundRaycastAmplitude;

        [Header("Hidden Config")]
        public int OrderSpriteofPlayer;

        [Header("Debug Options")]
		public bool EnableDebug = false;

		public SO_HiddenSubSM(){
			State = new HiddenSubSM(this);
			NodeType = PlayerState.Hidden;
		}
	}

}