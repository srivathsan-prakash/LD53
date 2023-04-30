using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObjectHolding : MonoBehaviour
{
	public Item item;

	private Item dropoffItem;

	private void OnTriggerEnter2D(Collider2D collision) {
		if(collision.CompareTag("Dropoff")) {
			dropoffItem = collision.GetComponentInChildren<Item>();
			Debug.Log($"{name} in the dropoff zone; Dropoff item {(dropoffItem == null ? "NULL" : dropoffItem.name)}");
		}
	}

	private void OnTriggerExit2D(Collider2D collision) {
		if(collision.CompareTag("Dropoff")) {
			dropoffItem = null;
		}
	}

	private void Update() {
		if(Input.GetKeyDown(KeyCode.Space) && dropoffItem != null) {
			Debug.Log($"{name} item is empty {item.IsEmpty()}, dropoff item is empty {dropoffItem.IsEmpty()}");
			if(item.IsEmpty() && !dropoffItem.IsEmpty()) {
				item.SetValues(dropoffItem.Components);
				dropoffItem.Clear();
			} else if (!item.IsEmpty() && dropoffItem.IsEmpty()) {
				dropoffItem.SetValues(item.Components);
				item.Clear();
			}
		}
	}
}
