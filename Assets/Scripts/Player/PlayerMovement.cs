using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public string axisName;
	public float speed;
	public float slipperySpeed;
	public Animator animator;
	public SpriteRenderer sprite;

	private bool isSlippery = false;

	private void Update() {
		// Get variables
		Vector3 distance = (isSlippery ? slipperySpeed : speed) * Time.deltaTime * GetInputValues(true);
		bool isAnimatorWalking = animator.GetBool("isWalking");
		bool isPlayerWalking = distance != Vector3.zero;
		
		// Set Movement and Animation states
		transform.position += distance;
		sprite.flipX = distance.x < 0;

		if (isAnimatorWalking != isPlayerWalking)
			animator.SetBool("isWalking", isPlayerWalking);

	}

	private Vector3 GetInputValues(bool raw) {
		return raw ? InputManager.GetAxisRaw(axisName) : InputManager.GetAxis(axisName);
	}

	public void MakeSlippery(bool slippery) {
		InputManager.SetSlippery(axisName, slippery);
		isSlippery = slippery;
	}
}
