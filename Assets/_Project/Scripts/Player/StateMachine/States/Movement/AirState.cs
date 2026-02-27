using UnityEngine;
using StateMachine;

namespace Player.Movement.States
{
    public class AirState : AState
    {

        private readonly SO_AirState _AirData;
        private PlayerController _player;

        public AirState(SO_State data) : base(data)
        {
            _AirData = _stateData as SO_AirState;
        }

        public override void Init(StateMachineController controller, AStateMachine parent = null)
        {
            base.Init(controller, parent);
            _player = controller as PlayerController;
        }

        public override void EnterState()
        {
            base.EnterState();
            Debug.Log("Enter to Air State");
        }

        public override void UpdateState()
        {
            base.UpdateState();
        }

        public override void FixedUpdateState()
        {
            base.FixedUpdateState();
        }

        public override void ExitState()
        {
            base.ExitState();
        }
    }
}
