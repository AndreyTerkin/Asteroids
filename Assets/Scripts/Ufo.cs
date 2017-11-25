using UnityEngine;

using AsteroidsLibrary.Movers;

public class Ufo : EngineSpaceObject
{
    [SerializeField]
    private float speed = 2.0f;
    [SerializeField]
    private int scoreForDestroy = 3;

    private IMovable mover;

    public override float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    public override int ScoresForDestroy
    {
        get { return scoreForDestroy; }
        set { scoreForDestroy = value; }
    }

    protected void Start()
    {
        Player player = FindObjectOfType<Player>();
        if (player != null)
            mover = new MoverRelativeConstantAim(player.SpaceObject, speed);
    }

    void FixedUpdate()
    {
        if (mover != null)
            transform.position = mover.UpdatePosition(transform.position);
    }
}
