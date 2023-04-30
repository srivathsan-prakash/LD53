using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ComponentRenderer
{
	public CompType type;
	public SpriteRenderer rend;

	public void SetSprite(Sprite s) {
		rend.sprite = s;
		rend.color = s == null ? Color.clear : Color.white;
	}
}

public class Item : MonoBehaviour
{
	public bool listenToChanges = false;
	public ComponentRenderer[] renderers;

    public Dictionary<CompType, int> Components;

    private const int defaultVariant = -1;

    private void Awake() {
        InitializeComponentsDictionary();
    }

    private void OnEnable() {
		if(listenToChanges) {
	        Events.UpdateItemComponent += UpdateComponent;
		}
    }

    private void OnDisable() {
        Events.UpdateItemComponent -= UpdateComponent;
    }

    private void UpdateComponent(CompType compType, int variant) {
        Components[compType] = variant;

        DisplayComponents();
    }

    private void InitializeComponentsDictionary() {
        Components = new Dictionary<CompType, int>();

        foreach (CompType type in Enum.GetValues(typeof(CompType)))
            Components.Add(type, defaultVariant);
    }

    private void DisplayComponents() {
		foreach(ComponentRenderer cRend in renderers) {
			if(Components.ContainsKey(cRend.type)) {
				cRend.SetSprite(ComponentManager.GetVariantSprite(cRend.type, Components.GetValueOrDefault(cRend.type)));
			}
		}

        string result = string.Empty;

        foreach (KeyValuePair<CompType, int> component in Components)
            result += component.Key + " : " + component.Value + " | ";

        Debug.Log(result);
    }

	public void Clear() {
		foreach(CompType type in Enum.GetValues(typeof(CompType))) {
			Components[type] = defaultVariant;
		}
		DisplayComponents();
	}

	public bool IsEmpty() {
		foreach(KeyValuePair<CompType, int> component in Components) {
			if(component.Value != defaultVariant) {
				return false;
			}
		}
		return true;
	}

	public void SetValues(Dictionary<CompType, int> dic) {
		Components = new Dictionary<CompType, int>(dic);
		DisplayComponents();
	}
}
