using StateMachine;

namespace Player {

	public enum PlayerState {
		Movement,
		Hidden,
		Death,
	}

	public enum MovementSate {
		Ground, //Puede Cambiar
		Crouch
	}

    public enum Hidden
    {
        Ground, //Puede Cambiar
        Crouch
    }

	public class BaseStateMachine : AStateMachine {
		private PlayerController _player;

		public BaseStateMachine(SO_StateMachine data) : base(data) { }

		public override void Init(StateMachineController controller, AStateMachine parent = null) {
			base.Init(controller, parent);
			_player = controller as PlayerController;
		}

		public override void UpdateState() {
			base.UpdateState();
		}

	}
}