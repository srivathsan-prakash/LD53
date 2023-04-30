using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

[Serializable]
public class ComponentImage
{
	public CompType type;
	public Image img;

	public void SetSprite(Sprite s) {
		img.sprite = s;
		img.color = s == null ? Color.clear : Color.white;
	}
}

public class Customer : MonoBehaviour
{
	public Image moodIndicator;
	public Slider patienceSlider;
	public ComponentImage[] itemImgs;

	public float maxPatience;
	public float patienceDecay;
	
	private Dictionary<CompType, int> order = new Dictionary<CompType, int>();
	private float currentPatience;

	private void Start() {
		RandomizeOrder();
	}

	private void RandomizeOrder() {
		foreach (CompType type in Enum.GetValues(typeof(CompType))) {
			int variant = UnityEngine.Random.Range(0, ComponentManager.ComponentNumber(type));
			order.Add(type, variant);
		}
		ShowItem();
	}

	private void ShowItem() {
		foreach (ComponentImage cImg in itemImgs) {
			if (order.ContainsKey(cImg.type)) {
				cImg.SetSprite(ComponentManager.GetVariantSprite(cImg.type, order.GetValueOrDefault(cImg.type)));
			}
		}
	}

	public void GiveItem(Dictionary<CompType, int> item) {
		//Check if the item is exactly correct
		if(item.Count == order.Count) {
			bool success = true;
			foreach(CompType type in Enum.GetValues(typeof(CompType))) {
				success = success && item[type] == order[type];
			}
			if(success) {
				OrderCorrect();
			} else {
				OrderFailed();
			}
		} else {
			OrderFailed();
		}
	}

	private void OrderFailed() {
		//Negative points, leave
		Debug.Log("Order failed!");
	}

	private void OrderCorrect() {
		//Apply points multipliers, leave
		Debug.Log("Order correct!");
	}
}
