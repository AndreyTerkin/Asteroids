using UnityEngine;
using System.Collections;

public class Asteroid : SpaceObject
{
    public GameObject fragment;

    [SerializeField]
    private int fragmentCount = 2;

    protected override void Explode()
    {
        for (int i = 0; i < fragmentCount; ++i)
        {
            Vector2 direction = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
            GameObject clone = Instantiate(fragment, transform.position, Quaternion.identity) as GameObject;
            Rigidbody2D rb = clone.GetComponent<Rigidbody2D>();
            rb.AddForce(direction * 5.0f, ForceMode2D.Impulse);
        }
        base.Explode();
    }
}
