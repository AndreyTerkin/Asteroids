using UnityEngine;

public class AsteroidFragment : SpaceObject
{
    [SerializeField]
    private int scoreForDestroy = 1;

    protected override void Explode()
    {
        base.Explode();
        base.OnSpaceObjectDestroyed(scoreForDestroy);
    }
}
