using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ComponentManager : MonoBehaviour
{
	public ComponentVariants[] componentVariants;

	private static ComponentManager instance;

	private void Awake() {
		if(instance == null) {
			instance = this;
		}
	}

	public static Sprite GetVariantSprite(CompType type, int i) {
		ComponentVariants v = instance.GetVariants(type);
		if(v != null && i >= 0 && i < v.variants.Length) {
			return v.variants[i];
		} else {
			return null;
		}
	}

	private ComponentVariants GetVariants(CompType type) {
		return componentVariants.FirstOrDefault(x => x.type == type);
	}
}
