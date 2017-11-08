using UnityEngine;
using System.Collections;

public class Ufo : SpaceObject
{
    [SerializeField]
    private float speed = 2.0f;

    private Vector3 _aimPosition;

    protected void Start()
    {
        Player player = FindObjectOfType<Player>();
        if (player)
            player.PositionChangedHandler += ChangeDirection;
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
}
