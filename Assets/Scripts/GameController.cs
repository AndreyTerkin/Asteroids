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
        Screen.SetResolution(800, 600, true);

        Boundary boundary = FindObjectOfType<Boundary>();
        if (boundary)
            border = boundary.border;

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
                AsteroidFactory();
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
                UfoFactory();
                yield return new WaitForSeconds(time);
            }
        }
    }

    void AsteroidFactory()
    {
        Vector2 direction = new Vector2(1.0f, 1.0f);
        GameObject clone = _spaceObjectFactory.Create(asteroid, ref direction);
        Rigidbody2D rb = clone.GetComponent<Rigidbody2D>();
        rb.AddForce(direction * 3.0f, ForceMode2D.Impulse);
    }

    void UfoFactory()
    {
        Vector2 direction = new Vector2(1.0f, 1.0f);
        GameObject clone = _spaceObjectFactory.Create(ufo, ref direction);
        Rigidbody2D rb = clone.GetComponent<Rigidbody2D>();
        rb.AddForce(direction * 2.0f, ForceMode2D.Impulse);
    }
}
