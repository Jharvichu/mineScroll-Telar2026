using UnityEngine;
using StateMachine;

namespace Player {

	[RequireComponent(typeof(Rigidbody2D))]
	public class PlayerController : StateMachineController {

		public Rigidbody2D Rigidbody2D { private set; get; }
		public Animator Animator { private set; get; }
		public float FacingDirection => transform.localScale.x > 0 ? 1f : -1f;

		protected override void Awake() {
			Rigidbody2D = GetComponent<Rigidbody2D>();
			Animator = GetComponent<Animator>();
			base.Awake();
		}

		public void FlipSprite(float directionX) {
			Vector3 scale = transform.localScale;

			if (directionX < 0 && scale.x > 0) {
				scale.x = -1f;
				transform.localScale = scale;
			}
			else if (directionX > 0 && scale.x < 0) {
				scale.x = 1f;
				transform.localScale = scale;
			}
		}

		void OnTriggerEnter2D(Collider2D collision) {

		}

		public void Restart() {
			base.Awake();
			ResetMachine();
		}

	}

}