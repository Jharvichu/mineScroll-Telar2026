using UnityEngine;
using StateMachine;

namespace Player.Hidden.States
{
    [CreateAssetMenu(
    fileName = "SO_CrouchState",
    menuName = "Player/Hidden States/Crouch State",
    order = 2)]
    public class SO_CrouchState : SO_State
    {
        public float HorizontalVelocity;

        [Header("Ground Raycast")]
        public LayerMask GroundLayer;
        public float GroundRaycastDistance;
        public float GroundRaycastAmplitude;

        [Header("Hiding Spot Detection")]
        public LayerMask HidingSpotLayer;
        public float DetectionRaycastDistance;
        public float DetectionRaycastAmplitude;

        public LayerMask HidingSpotCrouchLayer;
        public float DetectionRaycastOffSetY;
        public float DetectionRaycastSizeY;

        [Header("Ceiling Boxcast")]
        public LayerMask CeilingLayer;
        public Vector2 CeilingBoxSize;
        public float CeilingBoxOffset;
        public float CeilingCheckDistance;

        [Header("Collider Crouch")]
        public Vector2 ColliderBoxSize;
        public Vector2 ColliderBoxOffset;

        public SO_CrouchState()
        {
            State = new CrouchState(this);
            NodeType = HiddenState.Crouch;
        }

    }
}