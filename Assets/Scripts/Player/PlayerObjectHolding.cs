using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObjectHolding : MonoBehaviour
{
	public Item item;

	private Item dropoffItem;
	private Customer customer;

	private void OnTriggerEnter2D(Collider2D collision) {
		if(collision.CompareTag("Dropoff")) {
			dropoffItem = collision.GetComponentInChildren<Item>();
		} else if (collision.CompareTag("Customer")) {
			customer = collision.GetComponent<Customer>();
		}
	}

	private void OnTriggerExit2D(Collider2D collision) {
		if(collision.CompareTag("Dropoff")) {
			dropoffItem = null;
		} else if (collision.CompareTag("Customer")) {
			customer = null;
		}
	}

	private void Update() {
		if(Input.GetKeyDown(KeyCode.Space)) {
			if(customer != null) {
				if(!item.IsEmpty()) {
					customer.GiveItem(item.Components);
					item.Clear();
				}
			} else if(dropoffItem != null) { //if we're in both triggers, favor the customer
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
}
