using UnityEngine;

public class Comp : MonoBehaviour
{
    public CompType type;
    public int variant;
	[SerializeField] private SpriteRenderer rend;

	private void Start() {
		rend.sprite = ComponentManager.GetVariantSprite(type, variant);
	}
}
