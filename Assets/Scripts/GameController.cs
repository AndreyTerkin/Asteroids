using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Assets.Scripts.Factories;

public class GameController : MonoBehaviour
{
    public Text scorePanel;
    public GameObject menu;
    public float asteroidSpeed = 3.0f;
    public RectTransform laserAccumulatorCharge;

    [SerializeField]
    private float minAsteroidSpawnTime = 1.0f;
    [SerializeField]
    private float maxAsteroidSpawnTime = 5.0f;
    [SerializeField]
    private float minUfoSpawnTime = 4.0f;
    [SerializeField]
    private float maxUfoSpawnTime = 12.0f;

    private SpaceObjectFactory spaceObjectFactory;
    private RepresentationManager representationManager;

    private Border border;
    private GameObject player;
    private GameObject asteroid;
    private GameObject ufo;

    private int waveNum;
    private int enemiesPerWave;
    private int score;

    private void Start()
    {
        waveNum = 0;
        enemiesPerWave = 10;
        score = 0;
        UpdateScore(0);

        representationManager = GetComponent<RepresentationManager>();
        player = representationManager.GetGameObjectOfType<Player>();
        asteroid = representationManager.GetGameObjectOfType<Asteroid>();
        ufo = representationManager.GetGameObjectOfType<Ufo>();

        if (menu != null)
            menu.SetActive(false);

        Boundary boundary = FindObjectOfType<Boundary>();
        if (boundary)
            border = boundary.border;

        if (player)
        {
            var instance = Instantiate(player, new Vector3(0, 0, 0), Quaternion.identity);
            Player playerScript = instance.GetComponent<Player>();
            if (playerScript)
            {
                UpdateLaserAccumulatorDisplay(1, 1); // Reset value
                playerScript.LaserChargeChangedEvent += UpdateLaserAccumulatorDisplay;
                (playerScript as SpaceObject).SpaceObjectDestroyedEvent += GameOver;
            }
        }

        spaceObjectFactory = new SpaceObjectFactory(transform, border);
        if (asteroid != null)
            StartCoroutine(SpawnAsteroid());
        if (ufo != null)
            StartCoroutine(SpawnUfo());
    }

    private void DestroyObjectsOfTag(string tag)
    {
        var objects = GameObject.FindGameObjectsWithTag(tag);
        foreach (var obj in objects)
        {
            Destroy(obj);
        }
    }

    private IEnumerator SpawnAsteroid()
    {
        while (true)
        {
            waveNum++;

            for (int i = 0; i < enemiesPerWave * waveNum; i++)
            {
                float time = Random.Range(minAsteroidSpawnTime / waveNum,
                                          maxAsteroidSpawnTime / waveNum);
                spaceObjectFactory.Create(asteroid, asteroidSpeed);
                yield return new WaitForSeconds(time);
            }
        }
    }

    private IEnumerator SpawnUfo()
    {
        while (true)
        {
            waveNum++;

            for (int i = 0; i < enemiesPerWave * waveNum; i++)
            {
                float time = Random.Range(minUfoSpawnTime / waveNum,
                                          maxUfoSpawnTime / waveNum);
                spaceObjectFactory.Create(ufo);
                yield return new WaitForSeconds(time);
            }
        }
    }

    public void UpdateScore(int scoreIncrement)
    {
        score += scoreIncrement;
        scorePanel.text = "Score: " + score;
    }

    private void UpdateLaserAccumulatorDisplay(int charge, int maxCharge)
    {
        if (laserAccumulatorCharge == null)
            return;

        laserAccumulatorCharge.localScale = new Vector2((float)charge / (float)maxCharge, 1.0f);
    }

    // TODO: replace score to DestroyedEventArgs class or something,
    // because score isn't used here
    public void GameOver(int score)
    {
        StopAllCoroutines();
        if (menu != null)
            menu.SetActive(true);
    }

    public void RestartGame()
    {
        DestroyObjectsOfTag("Player");
        DestroyObjectsOfTag("Space object");
        DestroyObjectsOfTag("Weapon");
        Start();
    }

    public void ChangeGameView()
    {
        representationManager.ChangeRepresentation();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
