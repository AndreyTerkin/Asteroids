using UnityEngine;
using Assets.Scripts.Factories;

public class Asteroid : EngineSpaceObject
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
        SpaceObject.OnPositionChanged(transform.position);
    }

    public override void Explode()
    {
        for (int i = 0; i < fragmentCount; ++i)
        {
            if (fragment == null) 
                continue;

            SpaceObjectFactory.Create(fragment, transform.position, asteroidFragmentSpeed);
        }
        SpaceObject.OnSpaceObjectDestroyed(scoreForDestroy);
        Destroy(gameObject);
    }
}
