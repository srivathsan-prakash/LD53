using UnityEngine;

public class Comp : MonoBehaviour
{
    [SerializeField] private CompType type;
    [SerializeField] private int variant;

    private bool isActive = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            isActive = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            isActive = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isActive)
            {
                Debug.LogFormat("Space pressed near component: {0}, variant: {1}", type, variant);
                Events.UpdateItemComponent?.Invoke(type, variant);
            }
        }
    }
}
