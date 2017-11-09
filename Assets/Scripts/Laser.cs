using UnityEngine;

class Laser : MonoBehaviour
{
    [SerializeField]
    private float speed = 15.0f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * speed;
    }
}
