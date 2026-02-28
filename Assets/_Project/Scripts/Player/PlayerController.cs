using UnityEngine;
using StateMachine;

namespace Player {

	[RequireComponent(typeof(Rigidbody2D))]
	public class PlayerController : StateMachineController {

		public Rigidbody2D Rigidbody2D { private set; get; }
		public Animator Animator { private set; get; }

        public SpriteRenderer SpriteRenderer { private set; get; }
        public Collider2D CurrentHidingSpotCollider { private set; get; }

        public float FacingDirection => transform.localScale.x > 0 ? 1f : -1f;

        public bool canHide, isHidden, isCrouching;

		protected override void Awake() {
			Rigidbody2D = GetComponent<Rigidbody2D>();
			Animator = GetComponent<Animator>();
			SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
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

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("HiddenSpotDynamic"))
            {
                canHide = true;
                CurrentHidingSpotCollider = collision;
            }

            if (collision.gameObject.layer == LayerMask.NameToLayer("HiddenSpotStatic"))
            {
                canHide = true;
                CurrentHidingSpotCollider = collision;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("HiddenSpotDynamic"))
            {
                canHide = false;
                CurrentHidingSpotCollider = collision;
            }

            if (collision.gameObject.layer == LayerMask.NameToLayer("HiddenSpotStatic"))
            {
                canHide = false;
                CurrentHidingSpotCollider = null;
            }
        }

        public void Restart() {
			base.Awake();
			ResetMachine();
		}

	}

}