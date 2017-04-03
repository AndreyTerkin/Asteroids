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

        InvokeRepeating("AsteroidFactory", 0.0f, 1.0f);
    }

    void AsteroidFactory()
    {
        BoxCollider2D collider = asteroid.GetComponent<BoxCollider2D>();

        Vector3 position = new Vector3();
        Vector2 direction = new Vector2();

        InitSpawnParameters(collider, ref position, ref direction);

        Debug.Log("factory: " + position + " " + direction);

        GameObject clone = Instantiate(asteroid, position, Quaternion.identity) as GameObject;
        Rigidbody2D rb = clone.GetComponent<Rigidbody2D>();
        rb.AddForce(direction * 5.0f, ForceMode2D.Impulse);
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

        Debug.Log("init spawn: " + position + " " + direction);
    }
}
