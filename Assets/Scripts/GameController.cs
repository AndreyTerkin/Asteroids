using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Factories;

public class GameController : MonoBehaviour
{
    public GameObject asteroid;
    public GameObject ufo;
    public Text scorePanel;
    public GameObject menu;
    public int score;
    public RectTransform laserAccumulatorCharge;

    private SpaceObjectFactory _spaceObjectFactory;
    private RepresentationManager representationManager;

    private Border border;
    private Player player;

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

    private bool restartFlag;

    void Start()
    {
        restartFlag = false;

        representationManager = GetComponent<RepresentationManager>();

        if (menu != null)
            menu.SetActive(false);

        Boundary boundary = FindObjectOfType<Boundary>();
        if (boundary)
            border = boundary.border;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj)
        {
            player = playerObj.GetComponent<Player>();
            if (player)
            {
                player.LaserChargeChangedEvent += UpdateLaserAccumulatorDisplay;
                (player as SpaceObject).SpaceObjectDestroyedEvent += GameOver;
            }
        }

        _spaceObjectFactory = new SpaceObjectFactory(transform, border);

        if (asteroid != null)
            StartCoroutine(SpawnAsteroid());
        if (ufo != null)
            StartCoroutine(SpawnUfo());
    }

    void Update()
    {
        if (restartFlag)
        {
            PlayerPrefs.SetInt("Representation", (int)representationManager.Representation);
            SceneManager.LoadScene("Asteroids");
        }
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
        restartFlag = true;
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
