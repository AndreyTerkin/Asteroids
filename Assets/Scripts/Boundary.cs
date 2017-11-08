using UnityEngine;
using System.Collections;

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

        //Camera.main.
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag != "Player")
            Destroy(collider.gameObject);
    }
}

[System.Serializable]
public struct Border
{
    public float borderXmin;
    public float borderXmax;
    public float borderYmin;
    public float borderYmax;
}
