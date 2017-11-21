using UnityEngine;

public class SpaceObject : MonoBehaviour
{
    public delegate void SpaceObjectDestroyed(int score);
    public event SpaceObjectDestroyed SpaceObjectDestroyedEvent;

    public delegate void PositionChanged(Vector3 position);
    public event PositionChanged PositionChangedEvent;

    protected virtual void Explode()
    {
        // TODO: show explode
        Destroy(gameObject);
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

    protected virtual void OnSpaceObjectDestroyed(int score)
    {
        SpaceObjectDestroyed handler = SpaceObjectDestroyedEvent;
        if (handler != null)
        {
            handler(score);
        }
    }

    protected virtual void OnPositionChanged(Vector3 position)
    {
        PositionChanged handler = PositionChangedEvent;
        if (handler != null)
        {
            handler(position);
        }
    }
}
