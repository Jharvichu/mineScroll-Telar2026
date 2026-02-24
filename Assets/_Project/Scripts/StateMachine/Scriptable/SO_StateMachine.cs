using System.Collections.Generic;

namespace StateMachine {
	public abstract class SO_StateMachine : SO_Node {
		public SO_Node InitialState;
		public SO_Node CurrentState = null;
		public List<SO_Node> States;
	}
}