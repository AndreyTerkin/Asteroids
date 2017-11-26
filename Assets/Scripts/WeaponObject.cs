using System;
using UnityEngine;

using AsteroidsLibrary.SpaceObjects;

public class WeaponObject : MonoBehaviour, ISpaceObject
{
    [SerializeField]
    private float speed = 15.0f;

    protected Rigidbody2D rb;

    public virtual SpaceObject SpaceObject { get; set; }
    public virtual Vector2 Size { get; set; }
    public virtual int ScoresForDestroy { get; set; }

    public virtual float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * speed;
    }

    public void Explode(SpaceObject killer)
    {
        Destroy(gameObject);
    }
}
