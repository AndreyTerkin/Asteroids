using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    public GameObject asteroid;
    public GameObject ufo;

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
        BoxCollider2D collider = asteroid.GetComponent<BoxCollider2D>();

        Vector3 position = new Vector3();
        Vector2 direction = new Vector2();

        InitSpawnParameters(collider, ref position, ref direction);

        GameObject clone = Instantiate(asteroid, position, Quaternion.identity) as GameObject;
        Rigidbody2D rb = clone.GetComponent<Rigidbody2D>();
        rb.AddForce(direction * 3.0f, ForceMode2D.Impulse);
    }

    void UfoFactory()
    {
        BoxCollider2D collider = ufo.GetComponent<BoxCollider2D>();

        Vector3 position = new Vector3();
        Vector2 direction = new Vector2();

        InitSpawnParameters(collider, ref position, ref direction);

        Debug.Log("factory: " + position + " " + direction);

        GameObject clone = Instantiate(ufo, position, Quaternion.identity) as GameObject;
        Rigidbody2D rb = clone.GetComponent<Rigidbody2D>();
        rb.AddForce(direction * 2.0f, ForceMode2D.Impulse);
    }

    void InitSpawnParameters(BoxCollider2D collider, ref Vector3 position, ref Vector2 direction)
    {
        float deflection = Random.Range(-1.0f, 1.0f);
        float side = Random.Range(0.0f, 4.0f);
        
        if (side >= 0 && side < 1) // top
        {
            position = new Vector3(Random.Range(-border.borderXmax, border.borderXmax),
                                   border.borderYmax + collider.size.y,
                                   0.0f);
            direction = -transform.up + new Vector3(deflection, 0.0f);
        }
        else if (side >= 1 && side < 2) // bottom
        {
            position = new Vector3(Random.Range(-border.borderXmax, border.borderXmax),
                                   border.borderYmin - collider.size.y - errorOffset,
                                   0.0f);
            direction = transform.up + new Vector3(deflection, 0.0f);
        }
        else if (side >= 2 && side < 3) // right
        {
            position = new Vector3(border.borderXmax + collider.size.x,
                                   Random.Range(-border.borderYmax, border.borderYmax),
                                   0.0f);
            direction = -transform.right + new Vector3(0.0f, deflection);
        }
        else if (side >= 3 && side < 4) // left
        {
            position = new Vector3(border.borderXmin - collider.size.x,
                                   Random.Range(-border.borderYmax, border.borderYmax),
                                   0.0f);
            direction = transform.right + new Vector3(0.0f, deflection);
        }

        Debug.Log("Collider width: " + collider.size.x);
        Debug.Log("Spawning: " + border.borderXmax + " " + border.borderXmin);
    }
}
