using UnityEngine;
using System.Collections;

public class Asteroid : SpaceObject
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        Bullet bullet = collider.GetComponent<Bullet>();
        if (bullet)
            Explode();
    }
}
