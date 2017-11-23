using UnityEngine;

public class AsteroidFragment : EngineSpaceObject
{
    [SerializeField]
    private int scoreForDestroy = 1;

    public override int ScoresForDestroy
    {
        get { return scoreForDestroy; }
        set { scoreForDestroy = value; }
    }
}
