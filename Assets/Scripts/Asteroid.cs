using UnityEngine;

using Assets.Scripts;

public class Asteroid : EngineSpaceObject
{
    public GameObject fragment;

    [SerializeField]
    private float speed = 3.0f;
    public float asteroidFragmentSpeed = 4.0f;
    public int fragmentCount = 2;
    [SerializeField]
    private int scoreForDestroy = 2;

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

    private void FixedUpdate()
    {
        SpaceObject.OnPositionChanged(this, transform.position);
    }
}
