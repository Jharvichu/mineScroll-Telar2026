using UnityEngine;

namespace StateMachine {
	public abstract class SO_Node : ScriptableObject {
		public System.Enum NodeType;
		public INode State;

		public virtual void EnterState() {
			State.EnterState();
		}

		public virtual void UpdateState() {
			State.UpdateState();
		}

		public virtual void FixedUpdateState() {
			State.FixedUpdateState();
		}

		public virtual void ExitState() {
			State.ExitState();
		}

		public virtual void Init(StateMachineController controller, AStateMachine parent = null) {
			State.Init(controller, parent);
		}
	}
}