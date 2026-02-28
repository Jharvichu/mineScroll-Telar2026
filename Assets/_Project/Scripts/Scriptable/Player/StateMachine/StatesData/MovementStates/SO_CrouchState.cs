using UnityEngine;
using StateMachine;

namespace Player.Movement.States
{
    [CreateAssetMenu(
    fileName = "SO_CrouchState",
    menuName = "Player/Movement States/Crouch State",
    order = 2)]
    public class SO_CrouchState : SO_State
    {

        public float HorizontalVelocity;

        [Header("Ground Raycast")]
        public LayerMask GroundLayer;
        public float GroundRaycastDistance;
        public float GroundRaycastAmplitude;

        public SO_CrouchState()
        {
            State = new CrouchState(this);
            NodeType = MovementState.Crouch;
        }

    }
}