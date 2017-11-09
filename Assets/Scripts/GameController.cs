using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Assets.Scripts.Factories;

public class GameController : MonoBehaviour
{
    public GameObject asteroid;
    public GameObject ufo;

    private SpaceObjectFactory _spaceObjectFactory;

    private Border border;
    private Player player;

    private static float errorOffset = 0.25f;

    [SerializeField]
    private float minAsteroidSpawnTime = 1.0f;
    [SerializeField]
    private float maxAsteroidSpawnTime = 5.0f;
    [SerializeField]
    private float minUfoSpawnTime = 4.0f;
    [SerializeField]
    private float maxUfoSpawnTime = 12.0f;

    private int waveNum = 0;
    private int enemiesPerWave = 10;
    private int ufoCount = 1;
    private float spawnPeriod = 2.0f; 

    void Start()
    {
        Boundary boundary = FindObjectOfType<Boundary>();
        if (boundary)
            border = boundary.border;

        Player playerObj = FindObjectOfType<Player>();
        if (playerObj)
            player = playerObj;

        _spaceObjectFactory = new SpaceObjectFactory(transform, border);

        StartCoroutine(SpawnAsteroid());
        StartCoroutine(SpawnUfo());
    }

    IEnumerator SpawnAsteroid()
    {
        while (true)
        {
            waveNum++;

            for (int i = 0; i < enemiesPerWave * waveNum; i++)
            {
                float time = Random.Range(minAsteroidSpawnTime / waveNum,
                                          maxAsteroidSpawnTime / waveNum);
                Vector2 direction = new Vector2(1.0f, 1.0f);
                GameObject gameObject = _spaceObjectFactory.Create(asteroid, ref direction);
                if (gameObject)
                {
                    SpaceObject spaceObject = gameObject.GetComponent<SpaceObject>();
                    spaceObject.SpaceObjectDestroyedEvent += player.IncreaseScore;

                    Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
                    rb.AddForce(direction * 3.0f, ForceMode2D.Impulse);
                }
                yield return new WaitForSeconds(time);
            }
        }
    }

    IEnumerator SpawnUfo()
    {
        while (true)
        {
            waveNum++;

            for (int i = 0; i < enemiesPerWave * waveNum; i++)
            {
                float time = Random.Range(minUfoSpawnTime / waveNum,
                                          maxUfoSpawnTime / waveNum);
                Vector2 direction = new Vector2(1.0f, 1.0f);
                GameObject gameObject = _spaceObjectFactory.Create(ufo, ref direction);
                if (gameObject)
                {
                    SpaceObject spaceObject = gameObject.GetComponent<SpaceObject>();
                    spaceObject.SpaceObjectDestroyedEvent += player.IncreaseScore;
                }
                yield return new WaitForSeconds(time);
            }
        }
    }
}
