using UnityEngine;

public class AsteroidFragment : EngineSpaceObject
{
    [SerializeField]
    private float speed = 4.0f;
    [SerializeField]
    private int scoreForDestroy = 1;

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
}
