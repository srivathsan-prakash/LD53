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

		SetOutlines();
	}

	private void OnTriggerExit2D(Collider2D collision) {
		if(collision.CompareTag("Component")) {
			Comp c = collision.GetComponent<Comp>();
			if (component != null && component.Equals(c)) {
				SetOutline(component, false);
				component = null;
			}
		} else if (collision.CompareTag("Dropoff")) {
			SetOutline(dropoffItem.transform.parent.gameObject, false);
			dropoffItem = null;
		} else if (collision.CompareTag("Customer")) {
			SetOutline(customer, false);
			customer = null;
		} else if (collision.CompareTag("FireExtinguisher")) {
			SetOutline(extinguisher, false);
			extinguisher = null;
		} else if (collision.CompareTag("Fire")) {
			GameObject f = collision.gameObject;
			if(fire != null && fire.Equals(f)) {
				SetOutline(fire, false);
				fire = null;
			}
		} else if (collision.CompareTag("Spill")) {
			Spill s = collision.GetComponent<Spill>();
			if(spill != null && spill.Equals(s)) {
				SetOutline(spill, false);
				spill = null;
			}
		}

		SetOutlines();
	}

	private void SetOutline(GameObject obj, bool enable) {
		if(obj != null) {
			SpriteOutliner outline = obj.GetComponent<SpriteOutliner>();
			if (outline != null) {
				outline.EnableOutline(enable);
			}
		}
	}

	private void SetOutline(SpriteRenderer obj, bool enable) {
		if (obj != null) {
			SpriteOutliner outline = obj.GetComponent<SpriteOutliner>();
			if (outline != null) {
				outline.EnableOutline(enable);
			}
		}
	}

	private void SetOutline<T>(T obj, bool enable) where T: MonoBehaviour {
		if(obj != null) {
			SpriteOutliner outline = obj.GetComponent<SpriteOutliner>();
			if (outline != null) {
				outline.EnableOutline(enable);
			}
		}
	}

	private void CompareAndOutline<T>(T obj1, T obj2) where T: MonoBehaviour {
		if(obj1 != null && obj2 != null && !obj1.Equals(obj2)) {
			SetOutline(obj1, false);
			SetOutline(obj2, true);
		}
	}

	private void SetOutlines() {
		if(spill != null) {
			DisableAllOutlines();
			SetOutline(spill, true);
		} else if (component != null && !hasExtinguisher) {
			DisableAllOutlines();
			SetOutline(component, true);
		} else if (extinguisher != null) {
			DisableAllOutlines();
			SetOutline(extinguisher, true);
		} else if (hasExtinguisher && fire != null) {
			DisableAllOutlines();
			SetOutline(fire, true);
		} else if (customer != null) {
			DisableAllOutlines();
			SetOutline(customer, true);
		} else if (dropoffItem != null) {
			DisableAllOutlines();
			SetOutline(dropoffItem.transform.parent.gameObject, true);
		}
	}

	private void DisableAllOutlines() {
		SetOutline(spill, false);
		SetOutline(component, false);
		SetOutline(extinguisher, false);
		SetOutline(fire, false);
		SetOutline(customer, false);
		SetOutline(dropoffItem.transform.parent.gameObject, false);
	}
	
	private void Update() {
		if(Input.GetKeyDown(interactionKey)) {
			if (spill != null) {
				item.Clear();
				spill.IncrementLicks();
				anim.SetTrigger("Lick");
				Events.PlaySound?.Invoke("Lick");
			} else if (component != null && !hasExtinguisher) {
				item.UpdateComponent(component.type, component.variant);
				anim.SetTrigger("Pickup");
			} else if (extinguisher != null) {
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
				Events.PlaySound?.Invoke("FireGun");
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
					Events.PlaySound?.Invoke("PutDown");
				}
				else if (!item.IsEmpty() && dropoffItem.IsEmpty()) {
					dropoffItem.SetValues(item.Components);
					item.Clear();
					anim.SetTrigger("Pickup");
					Events.PlaySound?.Invoke("PutDown");
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
		Events.PlaySound?.Invoke("ToggleGun");
	}
}
