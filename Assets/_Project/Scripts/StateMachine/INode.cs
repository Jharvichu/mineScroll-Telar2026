namespace StateMachine {
	public interface INode {
		public abstract void EnterState();
		public abstract void UpdateState();
		public abstract void FixedUpdateState();
		public abstract void ExitState();
		public abstract void Init(StateMachineController controller, AStateMachine parent = null);
	}
}