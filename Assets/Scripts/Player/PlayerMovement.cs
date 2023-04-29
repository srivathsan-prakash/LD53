using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public enum MovementType { WASD, Arrows }

	public MovementType movement;
	public float speed;
	public bool isSlippery;

	private void Update() {
		float horiz = Input.GetAxis(GetHorizontalAxis());
		float vert = Input.GetAxis(GetVerticalAxis());
		Vector3 move = new Vector3(horiz, vert, 0);
		if(move.magnitude > 1) { move = move.normalized; }
		transform.position += move * speed * Time.deltaTime;
	}

	private string GetHorizontalAxis() {
		string axis = "Horizontal";
		axis += movement == MovementType.Arrows ? "_Arrows" : "_WASD";
		if(isSlippery) { axis += "_Slippery"; }
		return axis;
	}

	private string GetVerticalAxis() {
		string axis = "Vertical";
		axis += movement == MovementType.Arrows ? "_Arrows" : "_WASD";
		if(isSlippery) { axis += "_Slippery"; }
		return axis;
	}
}
