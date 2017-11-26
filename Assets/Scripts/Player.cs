using UnityEngine;

using AsteroidsLibrary;
using AsteroidsLibrary.SpaceObjects;

public class Player : EngineSpaceObject
{
    public delegate void LaserChargeChanged(int charge, int maxCharge);
    public event LaserChargeChanged LaserChargeChangedEvent;

    [SerializeField]
    private Transform shooter;
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private GameObject laserBolt;

    public int maxLaserAccumulatorCharge = 200;
    public int laserShotCost = 120;
    public int laserAccumulatorChargeSpeed = 1;

    private Border border;
    private Rigidbody2D rb;
    private Transform representation;

    private float firePeriod = 0.3f;
    private float previousShot = 0.0f;
    private int laserAccumulator;

    public override float Speed
    {
        get { return base.Speed; }
        set { base.Speed = value; }
    }

    protected void Awake ()
    {
        representation = transform.Find("Representation");
        rb = GetComponent<Rigidbody2D>();
    }

    protected void Start()
    {
        laserAccumulator = maxLaserAccumulatorCharge;

        Boundary boundary = FindObjectOfType<Boundary>();
        if (boundary)
            border = boundary.border;
    }

    protected override void FixedUpdate()
    {
        Fly();
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            Turn();

        rb.position = new Vector2(Mathf.Clamp(rb.position.x, border.xMin, border.xMax),
                                  Mathf.Clamp(rb.position.y, border.yMin, border.yMax));

        SpaceObject.OnPositionChanged(this, transform.position);

        if (laserAccumulator < maxLaserAccumulatorCharge)
        {
            laserAccumulator += laserAccumulatorChargeSpeed;
            if (LaserChargeChangedEvent != null)
                LaserChargeChangedEvent(laserAccumulator, maxLaserAccumulatorCharge);
        }
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time > previousShot)
        {
            Shoot();
        }
        if (Input.GetButton("Fire2") && Time.time > previousShot && laserAccumulator >= laserShotCost)
        {
            LaserShoot();
        }
    }

    private void Fly()
    {
        Vector3 direct = transform.right * Input.GetAxisRaw("Horizontal") +
                            transform.up * Input.GetAxisRaw("Vertical");

        transform.position = Vector3.MoveTowards(transform.position,
                                                 transform.position + direct,
                                                 Speed * Time.deltaTime);
    }

    private void Turn()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);

        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg - 90;

        representation.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void Shoot()
    {
        if (bullet == null)
            return;

        GameObject shot = Instantiate(bullet, shooter.position, shooter.rotation);
        shot.transform.parent = gameObject.transform.parent;

        SpaceObject spaceObject = ObjectSpawner.SpawnOnPosition(SpaceObjectTypes.Bullet, shooter.position, false);
        var shotObj = shot.GetComponent<ISpaceObject>();
        shotObj.SpaceObject = spaceObject;

        previousShot = Time.time + firePeriod;
    }

    private void LaserShoot()
    {
        if (laserBolt == null)
            return;

        GameObject shot = Instantiate(laserBolt, shooter.position, shooter.rotation);
        shot.transform.parent = gameObject.transform.parent;

        SpaceObject spaceObject = ObjectSpawner.SpawnOnPosition(SpaceObjectTypes.Laser, shooter.position, false);
        var shotObj = shot.GetComponent<ISpaceObject>();
        shotObj.SpaceObject = spaceObject;

        previousShot = Time.time + firePeriod;
        laserAccumulator -= laserShotCost;
        if (LaserChargeChangedEvent != null)
            LaserChargeChangedEvent(laserAccumulator, maxLaserAccumulatorCharge);
    }

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Space object")
        {
            ISpaceObject spaceObj = collider.gameObject.GetComponent<ISpaceObject>();
            if (spaceObj != null)
                Explode(spaceObj.SpaceObject);
        }
    }
}
