using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ComponentVariant", menuName = "ScriptableObjects/ComponentVariants", order = 0)]
public class ComponentVariants : ScriptableObject
{
	public CompType type;
	public Sprite[] variants;
}
