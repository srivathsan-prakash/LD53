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
			Debug.Log($"{name} in range for {(component != null ? component.name : "NULL")}");
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
			Comp c = collision.GetComponent<Comp>();
			if (component != null && component.Equals(c)) {
				component = null;
			}
		} else if (collision.CompareTag("Dropoff")) {
			dropoffItem = null;
		} else if (collision.CompareTag("Customer")) {
			customer = null;
		} else if (collision.CompareTag("FireExtinguisher")) {
			extinguisher = null;
		} else if (collision.CompareTag("Fire")) {
			GameObject f = collision.gameObject;
			if(fire != null && fire.Equals(f)) {
				fire = null;
			}
		} else if (collision.CompareTag("Spill")) {
			Spill s = collision.GetComponent<Spill>();
			if(spill != null && spill.Equals(s)) {
				spill = null;
			}
		}
	}
	
	private void Update() {
		if(Input.GetKeyDown(interactionKey)) {
			if (component != null && !hasExtinguisher) {
				item.UpdateComponent(component.type, component.variant);
			}  else if (extinguisher != null) {
				if(!hasExtinguisher) {
					item.Clear();
				}
				extinguisher.enabled = !extinguisher.enabled;
				hasExtinguisher = !hasExtinguisher;
			} else if (fire != null && hasExtinguisher) {
				Events.FireExtinguished?.Invoke();
				Destroy(fire);
			} else if (spill != null) {
				item.Clear();
				spill.IncrementLicks();
			} else if (customer != null) {
				if (!item.IsEmpty()) {
					customer.GiveItem(item.Components);
					item.Clear();
				}
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
