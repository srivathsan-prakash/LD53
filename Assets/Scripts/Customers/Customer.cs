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
	[Header("UI")]
	public GameObject itemHolder;
	public ComponentImage[] itemImgs;

	[Header("Patience")]
	public Image moodIndicator;
	public Slider patienceSlider;
	public Image sliderFill;
	public Sprite happyImg;
	public Color happyColor = Color.green;
	public Sprite mehImg;
	[Range(0, 1)] public float mehThreshold;
	public Color mehColor = Color.yellow;
	public Sprite angryImg;
	[Range(0, 1)] public float angryThreshold;
	public Color angryColor = Color.red;

	public float maxPatience;
	public float patienceDecayFront;
	public float patienceDecayBehind;
	
	private Dictionary<CompType, int> order = new Dictionary<CompType, int>();
	private float currentPatience;
	private float currentPatienceDecay;
	private bool isFront = false;

	private void Start() {
		RandomizeOrder();
		patienceSlider.maxValue = maxPatience;
		currentPatience = maxPatience;
		UpdateSliderVisual();
		currentPatienceDecay = patienceDecayBehind;
	}

	private void RandomizeOrder() {
		foreach (CompType type in Enum.GetValues(typeof(CompType))) {
			int variant = UnityEngine.Random.Range(0, ComponentManager.ComponentNumber(type));
			order.Add(type, variant);
		}
		if(isFront) {
			ShowItem();
		} else {
			itemHolder.SetActive(false);
		}
	}

	private void ShowItem() {
		itemHolder.SetActive(true);
		foreach (ComponentImage cImg in itemImgs) {
			if (order.ContainsKey(cImg.type)) {
				cImg.SetSprite(ComponentManager.GetVariantSprite(cImg.type, order.GetValueOrDefault(cImg.type)));
			}
		}
	}

	private void UpdateSliderVisual() {
		patienceSlider.value = currentPatience;
		if(currentPatience > maxPatience * mehThreshold) {
			moodIndicator.sprite = happyImg;
			sliderFill.color = happyColor;
		} else if (currentPatience > maxPatience * angryThreshold) {
			moodIndicator.sprite = mehImg;
			sliderFill.color = mehColor;
		} else {
			moodIndicator.sprite = angryImg;
			sliderFill.color = angryColor;
		}
	}

	private void Update() {
		currentPatience -= currentPatienceDecay * Time.deltaTime;
		currentPatience = Math.Max(currentPatience, 0);
		if(currentPatience <= 0) {
			OrderFailed();
		}
		UpdateSliderVisual();
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
		Events.CustomerLeft?.Invoke();
		Destroy(gameObject);
	}

	private void OrderCorrect() {
		//Apply points multipliers, leave
		Debug.Log("Order correct!");
		Events.CustomerLeft?.Invoke();
		Destroy(gameObject);
	}

	public void SetAsFront() {
		isFront = true;
		currentPatienceDecay = patienceDecayFront;
		ShowItem();
	}
}
