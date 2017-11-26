using UnityEngine;

using AsteroidsLibrary.SpaceObjects;

public class EngineSpaceObject : MonoBehaviour, ISpaceObject
{
    [SerializeField]
    private float speed = 1.0f;
    [SerializeField]
    private int scoreForDestroy = 0;

    public virtual SpaceObject SpaceObject { get; set; }
    public virtual Vector2 Size { get; set; }

    public virtual float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    public virtual int ScoresForDestroy
    {
        get { return scoreForDestroy; }
        set { scoreForDestroy = value; }
    }

    protected virtual void FixedUpdate()
    {
        if (SpaceObject.Mover != null)
            transform.position = SpaceObject.Mover.UpdatePosition(transform.position);
        SpaceObject.OnPositionChanged(this, transform.position);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Weapon" || collider.tag == "Player")
        {
            Explode(SpaceObjectTypes.Player);
        }
    }

    public virtual void Explode(SpaceObjectTypes killer)
    {
        SpaceObject.OnSpaceObjectDestroyed(this, transform.position, killer);
        Destroy(gameObject);
    }
}
