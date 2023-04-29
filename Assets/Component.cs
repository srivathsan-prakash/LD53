using UnityEngine;

public class Component : MonoBehaviour
{
    [SerializeField] private CompType type;
    [SerializeField] private int variant;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (collision.CompareTag("Player"))
            {
                Debug.LogFormat("Space pressed near component: {0}, variant: {1}", type, variant);
                Events.UpdateItemComponent?.Invoke(type, variant);
            }
        }
    }
}
