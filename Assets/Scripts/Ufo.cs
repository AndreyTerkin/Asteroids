using UnityEngine;
using System.Collections;

public class Ufo : SpaceObject
{
    [SerializeField]
    private float speed = 2.0f;
    [SerializeField]
    private int scoreForDestroy = 3;

    private Vector3 _aimPosition;

    protected void Start()
    {
        Player player = FindObjectOfType<Player>();
        if (player)
            player.PositionChangedEvent += ChangeDirection;
    }

    void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position,
                                                 _aimPosition,
                                                 speed * Time.deltaTime);
    }

    private void ChangeDirection(Vector3 position)
    {
        _aimPosition = position;
    }

    protected override void Explode()
    {
        base.OnSpaceObjectDestroyed(scoreForDestroy);
        base.Explode();
    }
}
