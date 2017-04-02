using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    public GameObject asteroid;

    [SerializeField]
    private Vector3 spawnCoordinates;

    private Border border;

    private static float errorOffset = 0.25f;

    void Start()
    {
        Boundary boundary = FindObjectOfType<Boundary>();
        if (boundary)
            border = boundary.border;

        InvokeRepeating("SpawnAsteroid", 0.0f, 1.0f);
    }

    void SpawnAsteroid()
    {
        BoxCollider2D bc = asteroid.GetComponent<BoxCollider2D>();

        Vector3 position = new Vector3();
        Vector2 direction = new Vector2();

        float rand = Random.Range(0.0f, 4.0f);

        if (rand >= 0 && rand < 1) //top
        {
            position = new Vector3(Random.Range(-border.borderXmax, border.borderXmax),
                                   border.borderYmax + bc.size.y,
                                   0.0f);
            direction = - transform.up;
        }
        else if (rand >= 1 && rand < 2) //bottom
        {
            position = new Vector3(Random.Range(-border.borderXmax, border.borderXmax),
                                   border.borderYmin - bc.size.y - errorOffset,
                                   0.0f);
            direction = transform.up;
        }
        else if (rand >= 2 && rand < 3) //right
        {
            position = new Vector3(border.borderXmax + bc.size.x,
                                   Random.Range(-border.borderYmax, border.borderYmax),
                                   0.0f);
            direction = - transform.right;
        }
        else if (rand >= 3 && rand < 4) //left
        {
            position = new Vector3(border.borderXmin - bc.size.x,
                                   Random.Range(-border.borderYmax, border.borderYmax),
                                   0.0f);
            direction = transform.right;
        }

        GameObject clone = Instantiate(asteroid, position, Quaternion.identity) as GameObject;
        Rigidbody2D rb = clone.GetComponent<Rigidbody2D>();
        rb.AddForce(direction * 5.0f, ForceMode2D.Impulse);
    }
}
