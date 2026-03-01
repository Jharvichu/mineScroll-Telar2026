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

        [Header("Ceiling Boxcast")]
        public LayerMask CeilingLayer;      // Qué capa representa el techo o los obstáculos sobre el jugador
        public Vector2 CeilingBoxSize;      // Tamaño del BoxCast usado para detectar el techo
        public float CeilingBoxOffset;     // Desplazamiento vertical desde el centro del jugador (hacia arriba)
        public float CeilingCheckDistance;  // Distancia máxima del BoxCast hacia arriba

        public SO_CrouchState()
        {
            State = new CrouchState(this);
            NodeType = MovementState.Crouch;
        }

    }
}