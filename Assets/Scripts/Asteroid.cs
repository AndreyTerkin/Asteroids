using UnityEngine;
using Assets.Scripts.Factories;

public class Asteroid : SpaceObject
{
    public GameObject fragment;

    [SerializeField]
    private float asteroidFragmentSpeed = 4.0f;
    [SerializeField]
    private int fragmentCount = 2;
    [SerializeField]
    private int scoreForDestroy = 2;

    private void FixedUpdate()
    {
        OnPositionChanged(transform.position);
    }

    protected override void Explode()
    {
        for (int i = 0; i < fragmentCount; ++i)
        {
            if (fragment == null) 
                continue;

            SpaceObjectFactory.Create(fragment, transform.position, asteroidFragmentSpeed);
        }
        base.OnSpaceObjectDestroyed(scoreForDestroy);
        base.Explode();
    }
}
