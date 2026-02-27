using UnityEngine;

namespace StateMachine {
	public abstract class StateMachineController : MonoBehaviour {
		[SerializeField] SO_StateMachine _rootStateMachine;

		protected virtual void Awake() {
			var _ = _rootStateMachine.State as AStateMachine;
			_rootStateMachine.CurrentState = null;
			_.Init(this);
		}

		protected virtual void Start() {
			var _ = _rootStateMachine.State as AStateMachine;
			_.ChangeState(_rootStateMachine.InitialState.NodeType);
		}

		protected virtual void Update() {
			_rootStateMachine.UpdateState();
		}

		protected virtual void FixedUpdate() {
			_rootStateMachine.FixedUpdateState();
		}

		public System.Enum GetCurrentState() {
			var currentMachine = _rootStateMachine.State as AStateMachine;
			System.Enum currentState = null;

			while (currentMachine is not null) {
				currentState = currentMachine.GetCurrentState();
				currentMachine = currentMachine.GetState(currentState).State as AStateMachine;
			}

			return currentState;
		}

		protected void ResetMachine() {
			var _ = _rootStateMachine.State as AStateMachine;
			_.ChangeState(_rootStateMachine.InitialState.NodeType);
		}
	}

}