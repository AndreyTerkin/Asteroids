using UnityEngine;
using System.Collections;

public class Player : SpaceObject
{
    private SpriteRenderer spriteRenderer;
    private Transform spriteObject;

    [SerializeField]
    private float speed = 5.0f;
    [SerializeField]
    private Transform shooter;
    [SerializeField]
    private GameObject bullet;

    private float firePeriod = 0.5f;
    private float previousShot = 0.0f;

	void Awake ()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteObject = transform.FindChild("Sprite");
	}
	
	void FixedUpdate()
    {
        Fly();
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            Turn();
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
        Vector3 direction = transform.right * Input.GetAxisRaw("Horizontal") +
                    transform.up * Input.GetAxisRaw("Vertical");

        transform.position = Vector3.MoveTowards(transform.position,
                                                 transform.position + direction,
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
        GameObject shot = (GameObject)Instantiate(bullet, shooter.position, shooter.rotation);
        shot.transform.parent = gameObject.transform;
    }
}
