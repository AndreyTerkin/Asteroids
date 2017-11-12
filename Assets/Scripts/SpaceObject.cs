using UnityEngine;

public class SpaceObject : MonoBehaviour
{
    public delegate void SpaceObjectDestroyed(int score);
    public event SpaceObjectDestroyed SpaceObjectDestroyedEvent;

    protected virtual void Explode()
    {
        // TODO: show explode
        Destroy(gameObject);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
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
}
