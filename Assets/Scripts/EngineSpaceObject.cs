using AsteroidsLibrary.SpaceObjects;
using UnityEngine;

public class EngineSpaceObject : MonoBehaviour, ISpaceObject
{
    public SpaceObject SpaceObject { get; protected set; }
    public virtual int ScoresForDestroy { get; set; }
    public virtual Vector2 Size { get; set; }
    public virtual float Speed { get; set; }

    protected virtual void Awake()
    {
        SpaceObject = new SpaceObject();
    }

    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        // TODO: place for improvement
        if (collider.transform.parent != null && collider.transform.parent.parent != null)
        {
            var colliderParent = collider.transform.parent.parent;
            if (colliderParent.tag == "Weapon" || colliderParent.tag == "Player")
            {
                Explode();
            }
        }
    }

    public virtual void Explode()
    {
        SpaceObject.OnSpaceObjectDestroyed(this, ScoresForDestroy);
        Destroy(gameObject);
    }
}
