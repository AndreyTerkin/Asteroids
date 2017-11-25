using UnityEngine;

using Assets.Scripts;

public class Asteroid : EngineSpaceObject
{
    public GameObject fragment;

    public float speed = 3.0f;
    public float asteroidFragmentSpeed = 4.0f;
    public int fragmentCount = 2;
    public int scoreForDestroy = 2;

    public override int ScoresForDestroy
    {
        get { return scoreForDestroy; }
        set { scoreForDestroy = value; }
    }

    private void FixedUpdate()
    {
        SpaceObject.OnPositionChanged(this, transform.position);
    }

    public override void Explode()
    {
        for (int i = 0; i < fragmentCount; ++i)
        {
            if (fragment == null) 
                continue;

            SceneObjectSpawner.Create(fragment, transform.position, asteroidFragmentSpeed);
        }
        SpaceObject.OnSpaceObjectDestroyed(this, scoreForDestroy);
        Destroy(gameObject);
    }
}
