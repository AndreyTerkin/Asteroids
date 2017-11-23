using UnityEngine;

public class AsteroidFragment : EngineSpaceObject
{
    [SerializeField]
    private int scoreForDestroy = 1;

    public override void Explode()
    {
        SpaceObject.OnSpaceObjectDestroyed(scoreForDestroy);
        Destroy(gameObject);
    }
}
