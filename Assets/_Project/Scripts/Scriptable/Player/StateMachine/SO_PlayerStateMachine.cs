using UnityEngine;
using StateMachine;

namespace Player {

	[CreateAssetMenu(
		fileName = "SO_PlayerStateMachine", 
		menuName = "Player/Base State Machine")]
	public class SO_PlayerSM : SO_StateMachine {

		public SO_PlayerSM(){
			State = new BaseStateMachine(this);
		}
	}

}
