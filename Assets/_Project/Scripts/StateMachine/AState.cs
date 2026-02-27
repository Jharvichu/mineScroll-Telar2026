using UnityEngine;

namespace StateMachine {
	public abstract class AState : INode {
		protected AStateMachine _parent;
		protected StateMachineController _stateMachineController;
		protected SO_State _stateData;

		protected bool _rightInput;
		protected bool _leftInput;
		protected bool _upInput;
		protected bool _downInput;

		public AState(SO_State data) {
			_stateData = data;
		}

		public virtual void EnterState() { }

		public virtual void ExitState() { }

		public virtual void UpdateState() { }

		public virtual void FixedUpdateState() { }

		public virtual void Init(StateMachineController controller, AStateMachine parent = null) {
			_stateMachineController = controller;
			_parent = parent;
		}

		protected void GetInputs()
		{
			_rightInput = Input.GetKey(KeyCode.D);
			_leftInput = Input.GetKey(KeyCode.A);
			_upInput = Input.GetKey(KeyCode.W);
			_downInput = Input.GetKey(KeyCode.S);
		}

	}
}