using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObjectHolding : MonoBehaviour
{
	public KeyCode interactionKey;
	public Item item;
	public GameObject plate;
	public Animator anim;

	private Comp component;
	private Item dropoffItem;
	private Customer customer;
	private SpriteRenderer extinguisher;
	private bool hasExtinguisher = false;
	private GameObject fire;
	private Spill spill;
	private int regLayerIndex = 0;
	private int gunLayerIndex = 1;

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.CompareTag("Component")) {
			Comp c = collision.GetComponent<Comp>();
			CompareAndOutline(component, c);
			component = c;
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

		SetOutline(collision.gameObject, true);
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

		SetOutline(collision.gameObject, false);
	}

	private void SetOutline(GameObject obj, bool enable) {
		SpriteOutliner outline = obj.GetComponent<SpriteOutliner>();
		if (outline != null) {
			outline.EnableOutline(enable);
		}
	}

	private void CompareAndOutline<T>(T obj1, T obj2) where T: MonoBehaviour {
		if(obj1 != null && obj2 != null && !obj1.Equals(obj2)) {
			SetOutline(obj1.gameObject, false);
			SetOutline(obj2.gameObject, true);
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
				SetOutline(extinguisher.gameObject, extinguisher.enabled);
				ToggleGun(hasExtinguisher);
				anim.SetTrigger("Pickup");
			} else if (hasExtinguisher) {
				anim.SetTrigger("Fire");
				if (fire != null) {
					Events.FireExtinguished?.Invoke();
					Destroy(fire);
                }
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

	private void ToggleGun(bool isOn)
	{
		if (isOn)
		{
			anim.SetLayerWeight(regLayerIndex, 0);
			anim.SetLayerWeight(gunLayerIndex, 1);
			plate.gameObject.SetActive(false);
		}
		else
		{
			anim.SetLayerWeight(gunLayerIndex, 0);
			anim.SetLayerWeight(regLayerIndex, 1);
			plate.gameObject.SetActive(true);
		}
	}
}
