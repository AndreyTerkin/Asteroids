using UnityEngine;
using System.Collections;

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
        Bullet bullet = collider.GetComponent<Bullet>();
        if (bullet || collider.tag == "Player")
        {
            Explode();
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
