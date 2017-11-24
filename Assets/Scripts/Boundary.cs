using UnityEngine;

public class Boundary : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private RectTransform rectTransform;

    public Border border;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.size = new Vector2(rectTransform.rect.width, rectTransform.rect.height);
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.transform.parent != null && collider.transform.parent.parent != null)
        {
            var colliderParent = collider.transform.parent.parent;
            if (colliderParent.tag != "Player")
            {
                Destroy(colliderParent.gameObject);
            }
        }
    }
}

[System.Serializable]
public struct Border
{
    public float xMin;
    public float xMax;
    public float yMin;
    public float yMax;
}
