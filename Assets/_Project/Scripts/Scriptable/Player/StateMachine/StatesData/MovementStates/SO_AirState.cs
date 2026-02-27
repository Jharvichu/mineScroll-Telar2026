using UnityEngine;
using StateMachine;

namespace Player.Movement.States
{
    [CreateAssetMenu(
    fileName = "SO_AirState",
    menuName = "Player/Movement States/Air State",
    order = 1)]
    public class SO_AirState : SO_State
    {

        public float FallAceleration;

        public SO_AirState()
        {
            State = new AirState(this);
            NodeType = MovementState.Air;
        }

    }
} 