using UnityEngine;

public class Comp : MonoBehaviour
{
    [SerializeField] private CompType type;
    [SerializeField] private int variant;
	[SerializeField] private SpriteRenderer rend;

    private bool playerInRange = false;

	private void Start() {
		rend.sprite = ComponentManager.GetVariantSprite(type, variant);
	}

	private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            playerInRange = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (playerInRange)
            {
                Debug.LogFormat("Space pressed near component: {0}, variant: {1}", type, variant);
                Events.UpdateItemComponent?.Invoke(type, variant);
            }
        }
    }
}
