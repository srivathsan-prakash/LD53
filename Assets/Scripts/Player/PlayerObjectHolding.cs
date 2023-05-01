using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObjectHolding : MonoBehaviour
{
	public KeyCode interactionKey;
	public Item item;
	[SerializeField] private Animator anim;

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
			if (spill != null) {
				item.Clear();
				spill.IncrementLicks();
				anim.SetTrigger("Lick");
			} else if (component != null && !hasExtinguisher) {
				item.UpdateComponent(component.type, component.variant);
				anim.SetTrigger("Pickup");
			}  else if (extinguisher != null) {
				if(!hasExtinguisher) {
					item.Clear();
				}
				extinguisher.enabled = !extinguisher.enabled;
				hasExtinguisher = !hasExtinguisher;
				//Get/put down the gun here
				anim.SetTrigger("Pickup");
			} else if (fire != null && hasExtinguisher) {
				Events.FireExtinguished?.Invoke();
				Destroy(fire);
				//Use the gun here
			} else if (customer != null) {
				if (!item.IsEmpty()) {
					customer.GiveItem(item.Components);
					item.Clear();
				}
				anim.SetTrigger("Pickup");
			} else if (dropoffItem != null) {
				if (item.IsEmpty() && !dropoffItem.IsEmpty()) {
					item.SetValues(dropoffItem.Components);
					dropoffItem.Clear();
					anim.SetTrigger("Pickup");
				} else if (!item.IsEmpty() && dropoffItem.IsEmpty()) {
					dropoffItem.SetValues(item.Components);
					item.Clear();
					anim.SetTrigger("Pickup");
				}
			}
		}
	}
}
