using UnityEngine;
using System.Collections;

public class Boundary : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.size = new Vector2(rectTransform.rect.width, rectTransform.rect.height);
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        Destroy(collider.gameObject);
    }
}
