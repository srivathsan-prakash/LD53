using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public string axisName;
	public float speed;
	public float slipperySpeed;
	public Animator animator;

	private bool isSlippery = false;
	private bool isWalking = false;
	private bool hasGun = false;

	private void Update() {
		transform.position += GetInputValues(true) * (isSlippery ? slipperySpeed : speed) * Time.deltaTime;
		
	}

	private Vector3 GetInputValues(bool raw) {
		return raw ? InputManager.GetAxisRaw(axisName) : InputManager.GetAxis(axisName);
	}

	public void MakeSlippery(bool slippery) {
		InputManager.SetSlippery(axisName, slippery);
		isSlippery = slippery;
	}
}
