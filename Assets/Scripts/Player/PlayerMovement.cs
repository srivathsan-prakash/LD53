using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public string axisName;
	public float speed;
	public float slipperySpeed;
	[SerializeField] private Rigidbody2D rb = null;

	private bool isSlippery = false;

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
