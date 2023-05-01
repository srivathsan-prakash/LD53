using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public string axisName;
	public float speed;
	public float slipperySpeed;
	public Animator animator;
	public SpriteRenderer sprite;
	public Transform itemHolder;
	[SerializeField] private Rigidbody2D rb = null;

	private bool isSlippery = false;

	private void Update()
	{
		// Get variables
		Vector3 distance = (isSlippery ? slipperySpeed : speed) * Time.deltaTime * GetInputValues(true);
		bool isAnimatorWalking = animator.GetBool("isWalking");
		bool isPlayerWalking = distance != Vector3.zero;

		// Set Movement and Animation states
		if(isPlayerWalking && distance.x != 0 && sprite.flipX != distance.x < 0) {
			sprite.flipX = distance.x < 0;
		}
		if(itemHolder != null) {
			if((itemHolder.localPosition.x > 0 && sprite.flipX) || (itemHolder.localPosition.x < 0 && !sprite.flipX)) {
				itemHolder.localPosition = new Vector3(-itemHolder.localPosition.x, itemHolder.localPosition.y, 0);
			}
		}

		if (isAnimatorWalking != isPlayerWalking)
			animator.SetBool("isWalking", isPlayerWalking);
	}
	private void FixedUpdate() {
		rb.velocity = GetInputValues(true) * (isSlippery ? slipperySpeed : speed);
	}

	private Vector3 GetInputValues(bool raw) {
		return raw ? InputManager.GetAxisRaw(axisName) : InputManager.GetAxis(axisName);
	}

	public void MakeSlippery(bool slippery) {
		InputManager.SetSlippery(axisName, slippery);
		isSlippery = slippery;
	}
}
