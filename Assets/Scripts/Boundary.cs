using UnityEngine;

using AsteroidsLibrary;
using AsteroidsLibrary.SpaceObjects;

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
        if (collider.tag != "Player")
        {
            ISpaceObject spaceObject = collider.gameObject.GetComponent<ISpaceObject>();
            if (spaceObject != null)
                spaceObject.Explode(SpaceObjectTypes.Boundary);
        }
    }
}
