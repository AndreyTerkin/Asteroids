using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
	[SerializeField]
	private float speed = 15.0f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * speed;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        SpaceObject spaceObject = collider.GetComponent<SpaceObject>();

        if (spaceObject && spaceObject.transform != transform.parent)
            Destroy(gameObject);
    }
}
