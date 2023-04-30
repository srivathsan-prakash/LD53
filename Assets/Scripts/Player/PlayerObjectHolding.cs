using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObjectHolding : MonoBehaviour
{
	public KeyCode interactionKey;
	public Item item;

	private Comp component;
	private Item dropoffItem;
	private Customer customer;
	private SpriteRenderer extinguisher;
	private bool hasExtinguisher = false;
	private GameObject fire;
	private Spill spill;

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.CompareTag("Component")) {
			component = collision.GetComponent<Comp>();
		} else if (collision.CompareTag("Dropoff")) {
			dropoffItem = collision.GetComponentInChildren<Item>();
		} else if (collision.CompareTag("Customer")) {
			customer = collision.GetComponent<Customer>();
		} else if (collision.CompareTag("FireExtinguisher")) {
			extinguisher = collision.GetComponent<SpriteRenderer>();
		} else if (collision.CompareTag("Fire")) {
			fire = collision.gameObject;
		} else if (collision.CompareTag("Spill")) {
			spill = collision.GetComponent<Spill>();
		}
	}

	private void OnTriggerExit2D(Collider2D collision) {
		if(collision.CompareTag("Component")) {
			component = null;
		} else if (collision.CompareTag("Dropoff")) {
			dropoffItem = null;
		} else if (collision.CompareTag("Customer")) {
			customer = null;
		} else if (collision.CompareTag("FireExtinguisher")) {
			extinguisher = null;
		} else if (collision.CompareTag("Fire")) {
			fire = null;
		} else if (collision.CompareTag("Spill")) {
			spill = null;
		}
	}
	
	private void Update() {
		if(Input.GetKeyDown(interactionKey)) {
			if (component != null && !hasExtinguisher) {
				item.UpdateComponent(component.type, component.variant);
			} else if (customer != null) {
				if (!item.IsEmpty()) {
					customer.GiveItem(item.Components);
					item.Clear();
				}
			} else if (extinguisher != null) {
				extinguisher.enabled = !extinguisher.enabled;
				hasExtinguisher = !hasExtinguisher;
			} else if (fire != null && hasExtinguisher) {
				Destroy(fire);
			} else if (spill != null) {
				spill.IncrementLicks();
			} else if (dropoffItem != null) { //if we're in both triggers, favor the customer
				if (item.IsEmpty() && !dropoffItem.IsEmpty()) {
					item.SetValues(dropoffItem.Components);
					dropoffItem.Clear();
				} else if (!item.IsEmpty() && dropoffItem.IsEmpty()) {
					dropoffItem.SetValues(item.Components);
					item.Clear();
				}
			}
		}
	}
}
