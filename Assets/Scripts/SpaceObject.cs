using UnityEngine;
using System.Collections;

public class SpaceObject : MonoBehaviour
{
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
            Debug.Log("destroy from SpaceObjectw class " + collider.gameObject.name);
            Explode();
        }
    }
}
