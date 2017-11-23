using UnityEngine;
using System.Collections;
using Assets.Scripts.Movers;

public class Ufo : SpaceObject
{
    [SerializeField]
    private float speed = 2.0f;
    [SerializeField]
    private int scoreForDestroy = 3;

    private IMovable mover;

    protected void Start()
    {
        Player player = FindObjectOfType<Player>();
        if (player != null)
            mover = new MoverRelativeNearestObject(typeof(Asteroid), speed);
    }

    void FixedUpdate()
    {
        if (mover != null)
            transform.position = mover.UpdatePosition(transform.position);
    }

    protected override void Explode()
    {
        base.OnSpaceObjectDestroyed(scoreForDestroy);
        base.Explode();
    }
}
