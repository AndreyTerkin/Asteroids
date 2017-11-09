using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Assets.Scripts.Factories;

public class GameController : MonoBehaviour
{
    public GameObject asteroid;
    public GameObject ufo;
    public Text scorePanel;
    public int score;
    public RectTransform laserAccumulatorCharge;

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

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj)
        {
            player = playerObj.GetComponent<Player>();
            if (player)
                player.LaserChargeChangedEvent += UpdateLaserAccumulatorDisplay;
        }

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
                _spaceObjectFactory.Create(asteroid, 3.0f);
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
                _spaceObjectFactory.Create(ufo);
                yield return new WaitForSeconds(time);
            }
        }
    }

    public void UpdateScore(int scoreIncrement)
    {
        score += scoreIncrement;
        scorePanel.text = "Score: " + score;
    }

    public void UpdateLaserAccumulatorDisplay(int charge, int maxCharge)
    {
        if (laserAccumulatorCharge == null)
            return;

        laserAccumulatorCharge.localScale = new Vector2((float)charge / (float)maxCharge, 1.0f);
    }
}
