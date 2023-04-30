using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[Serializable]
public class InputAxis
{
	private enum KeyDown { Positive, Negative, Neither }

	private const float MINIMUM = -1;
	private const float NEUTRAL = 0;
	private const float MAXIMUM = 1;

	[SerializeField] private KeyCode positiveKey;
	[SerializeField] private KeyCode negativeKey;
	[SerializeField] private float gravity;
	[SerializeField] private float sensitivity;
	[SerializeField] private bool snap;

	private float value = NEUTRAL;
	private KeyDown key = KeyDown.Neither;

	public KeyCode PositiveKey { get { return positiveKey; } }
	public KeyCode NegativeKey { get { return negativeKey; } }
	public void SetGravity(float g) { gravity = g; }
	public void SetSensitivity(float s) { sensitivity = s; }
	public float ValueRaw { get { return value; } }
	public float Value {
		get {
			if(key == KeyDown.Positive) { return MAXIMUM; }
			else if (key == KeyDown.Negative) { return MINIMUM; }
			else { return NEUTRAL; }
		}
	}

	public void PositiveKeyDown() {
		if(key == KeyDown.Negative && snap) {
			value = NEUTRAL;
		}
		key = KeyDown.Positive;
	}

	public void NegativeKeyDown() {
		if(key == KeyDown.Positive && snap) {
			value = NEUTRAL;
		}
		key = KeyDown.Negative;
	}

	public void UpdateValue() {
		switch (key) {
			case KeyDown.Positive:
				if(value < MAXIMUM) {
					value += sensitivity * Time.deltaTime;
				}
				break;
			case KeyDown.Negative:
				if(value > MINIMUM) {
					value -= sensitivity * Time.deltaTime;
				}
				break;
			case KeyDown.Neither:
				if(!Mathf.Approximately(value, NEUTRAL)) {
					if(value < NEUTRAL) {
						value += gravity * Time.deltaTime;
						value = Mathf.Min(value, NEUTRAL);
					} else {
						value -= gravity * Time.deltaTime;
						value = Mathf.Max(value, NEUTRAL);
					}
				}
				break;
		}
		value = Mathf.Clamp(value, MINIMUM, MAXIMUM);
	}

	public void PositiveKeyUp(bool negKeyHeld) {
		if(key == KeyDown.Positive) {
			key = negKeyHeld ? KeyDown.Negative : KeyDown.Neither;
		}
	}

	public void NegativeKeyUp(bool posKeyHeld) {
		if (key == KeyDown.Negative) {
			key = posKeyHeld ? KeyDown.Positive : KeyDown.Neither;
		}
	}
}

[Serializable]
public class InputScheme
{
	public string axisName;
	public InputAxis horizontal;
	public InputAxis vertical;

	public Vector2 GetValueRaw() {
		Vector2 move = new Vector2(horizontal.ValueRaw, vertical.ValueRaw);
		if(move.magnitude > 1) { move = move.normalized; }
		return move;
	}

	public Vector2 GetValue() {
		Vector2 move = new Vector2(horizontal.Value, vertical.Value);
		if(move.magnitude > 1) { move = move.normalized; }
		return move;
	}

	public void Update() {
		horizontal.UpdateValue();
		vertical.UpdateValue();
	}

	public void SetGravity(float value) {
		horizontal.SetGravity(value);
		vertical.SetGravity(value);
	}
	
	public void SetSensitivity(float value) {
		horizontal.SetSensitivity(value);
		vertical.SetSensitivity(value);
	}
}

public class InputManager : MonoBehaviour
{
	public InputScheme[] axes;
	public float standardGravity = 100f;
	public float standardSensitivity = 100f;
	public float slipperyGravity = 0.5f;
	public float slipperySensitivity = 0.5f;

	private static InputManager instance;

	private void Awake() {
		if(instance == null) {
			instance = this;
		}
	}

	public static Vector2 GetAxis(string axisName) {
		InputScheme axis = instance.axes.FirstOrDefault(x => x.axisName == axisName);
		return axis == null ? Vector2.zero : axis.GetValue();
	}

	public static Vector2 GetAxisRaw(string axisName) {
		InputScheme axis = instance.axes.FirstOrDefault(x => x.axisName == axisName);
		return axis == null ? Vector2.zero : axis.GetValueRaw();
	}

	public static void SetSlippery(string axisName, bool isSlippery) {
		InputScheme axis = instance.axes.FirstOrDefault(x => x.axisName == axisName);
		if(axis != null) {
			axis.SetGravity(isSlippery ? instance.slipperyGravity : instance.standardGravity);
			axis.SetSensitivity(isSlippery ? instance.slipperySensitivity : instance.standardSensitivity);
		}
	}

	private void Update() {
		foreach(var a in axes) {
			CheckKeys(a);
			a.Update();
		}
	}

	private void CheckKeys(InputScheme scheme) {
		CheckKeys(scheme.horizontal);
		CheckKeys(scheme.vertical);
	}

	private void CheckKeys(InputAxis axis) {
		if(Input.GetKeyDown(axis.NegativeKey)) {
			axis.NegativeKeyDown();
		}
		if(Input.GetKeyDown(axis.PositiveKey)) {
			axis.PositiveKeyDown();
		}
		if(Input.GetKeyUp(axis.NegativeKey)) {
			axis.NegativeKeyUp(Input.GetKey(axis.PositiveKey));
		}
		if(Input.GetKeyUp(axis.PositiveKey)) {
			axis.PositiveKeyUp(Input.GetKey(axis.NegativeKey));
		}
	}
}
