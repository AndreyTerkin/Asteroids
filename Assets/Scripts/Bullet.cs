using UnityEngine;

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
        if (collider.transform.parent != null && collider.transform.parent.parent != null)
        {
            var colliderParent = collider.transform.parent.parent;
            if (colliderParent.tag == "Space object")
                Destroy(gameObject);
        }
    }
}
