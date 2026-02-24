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
		protected ActionData _actionPerformed;

		public AState(SO_State data) {
			_stateData = data;
		}

		public virtual void EnterState() {
			InputHandler.Instance.actionTrigger.AddListener(GetActionNInputs);
		}

		public virtual void ExitState() { }

		public virtual void UpdateState() { }

		public virtual void FixedUpdateState() { }

		public virtual void Init(StateMachineController controller, AStateMachine parent = null) {
			_stateMachineController = controller;
			_parent = parent;
		}

		private void GetActionNInputs(ActionData data) {
			_rightInput = data.inputs.Exists(element => {
				return element.button == Btn.Right && element.canHold;
			});
			_leftInput = data.inputs.Exists(element => { return element.button == Btn.Left && element.canHold; });
			_upInput = data.inputs.Exists(element => { return element.button == Btn.Up && element.canHold; });
			_downInput = data.inputs.Exists(element => { return element.button == Btn.Down && element.canHold; });

			_actionPerformed = data;

			// if (Input.GetKeyDown(KeyCode.S))
			// {
			//	_downInput = !_downInput;
			// }

		}
	}
}