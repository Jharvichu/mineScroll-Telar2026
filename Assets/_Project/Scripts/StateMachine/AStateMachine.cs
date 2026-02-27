using System.Collections.Generic;
using UnityEngine;

namespace StateMachine {
	public abstract class AStateMachine : INode {

		protected AStateMachine _parent;
		protected StateMachineController _stateMachineController;
		protected SO_StateMachine _stateMachineData;
		protected Dictionary<System.Enum, SO_Node> _statesDict;

		protected bool _upInput;
		protected bool _downInput;
		protected bool _modeInput;

		public AStateMachine(SO_StateMachine data) {
			_stateMachineData = data;
		}

		public virtual void EnterState() {
			_stateMachineData.CurrentState.EnterState();
		}

		public virtual void UpdateState() {
			GetInputs();
			_stateMachineData.CurrentState.UpdateState();
		}

		public virtual void FixedUpdateState() {
			_stateMachineData.CurrentState.FixedUpdateState();
		}

		public virtual void ExitState() {
			if (_stateMachineData.CurrentState != null)
				_stateMachineData.CurrentState.ExitState();
		}

		/// <summary>
		/// Initialize the states in them and the children nodes
		/// </summary>
		public virtual void Init(StateMachineController controller, AStateMachine parent = null) {
			_stateMachineController = controller;
			_parent = parent;
			_statesDict = new();
			foreach (var SO_Node in _stateMachineData.States) {
				if (SO_Node.State == null || SO_Node.NodeType == null) {
					Debug.LogError($"The state with name {SO_Node.name} it's not properly defined");
					continue;
				}
				_statesDict.Add(SO_Node.NodeType, SO_Node);
				SO_Node.Init(controller, this);
			}
		}

		/// <summary>
		/// Changes the current state of the state machine
		/// If the state is not found, logs an error
		/// </summary>
		/// <param name="nodeType"></param>
		public void ChangeState(System.Enum nodeType) {
			if (!_statesDict.ContainsKey(nodeType)) {
				Debug.LogError($"State machine of type {_stateMachineData.NodeType} does not contain the state {nodeType}");
				return;
			}

			if (nodeType.Equals(GetCurrentState())) return;

			if (_stateMachineData.CurrentState != null) _stateMachineData.CurrentState.ExitState();

			_stateMachineData.CurrentState = _statesDict[nodeType];
			_stateMachineData.CurrentState.EnterState();
		}

		public System.Enum GetCurrentState() {
			return _stateMachineData.CurrentState != null
				? _stateMachineData.CurrentState.NodeType
				: null;
		}

		public SO_Node GetState(System.Enum nodeType) {
			if (!_statesDict.ContainsKey(nodeType)) {
				Debug.LogError($"State machine of type {_stateMachineData.NodeType} does not contain the state {nodeType}");
				return null;
			}

			return _statesDict[nodeType];
		}

		private void GetInputs()
		{
			_upInput 	= Input.GetKey(KeyCode.W);
			_downInput 	= Input.GetKey(KeyCode.S);
			_modeInput 	= Input.GetKeyDown(KeyCode.Space);
		}
	}
}