﻿using UnityEngine;
using System.Collections;

public class Player : SpaceObject
{
    public delegate void PositionChanged(Vector3 position);
    public event PositionChanged PositionChangedEvent;

    private Rigidbody2D rb;
    private Transform spriteObject;

    [SerializeField]
    private float speed = 7.0f;
    [SerializeField]
    private Transform shooter;
    [SerializeField]
    private GameObject bullet;

    public int Score { get; set; }

    private Border border;

    private float firePeriod = 0.3f;
    private float previousShot = 0.0f;

    void Awake ()
    {
        spriteObject = transform.Find("Sprite");
        rb = GetComponent<Rigidbody2D>();
    }

    protected void Start()
    {
        Boundary boundary = FindObjectOfType<Boundary>();
        if (boundary)
            border = boundary.border;
    }

    void FixedUpdate()
    {
        Fly();
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            Turn();

        rb.position = new Vector2(Mathf.Clamp(rb.position.x, border.borderXmin, border.borderXmax),
                                  Mathf.Clamp(rb.position.y, border.borderYmin, border.borderYmax));

        if (PositionChangedEvent != null)
            PositionChangedEvent(transform.position);
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time > previousShot)
        {
            previousShot = Time.time + firePeriod;
            Shoot();
        }
    }

    private void Fly()
    {
        Vector3 direct = transform.right * Input.GetAxisRaw("Horizontal") +
                            transform.up * Input.GetAxisRaw("Vertical");

        transform.position = Vector3.MoveTowards(transform.position,
                                                 transform.position + direct,
                                                 speed * Time.deltaTime);
    }

    private void Turn()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);

        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg - 90;

        spriteObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void Shoot()
    {
        if (bullet == null)
            return;

        GameObject shot = Instantiate(bullet, shooter.position, shooter.rotation);
        shot.transform.parent = gameObject.transform;
    }

    public void IncreaseScore(int score)
    {
        Score += score;
        Debug.Log("Score is " + Score);
    }

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Space object")
        {
            Explode();
        }
    }
}
