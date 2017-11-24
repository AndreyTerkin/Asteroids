using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts.Factories;
using AsteroidsLibrary;
using AsteroidsLibrary.SpaceObjects;

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
        UpdateScore(this, null);

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
                playerScript.SpaceObject.SpaceObjectDestroyedEvent += GameOver;
            }
        }

        spaceObjectFactory = new SpaceObjectFactory(transform, border);

        Game.GetInstance().SpaceObjectSpawnEvent += SpawnObject;
        Game.GetInstance().MessageDelegateEvent += (string text) => { Debug.Log(text); };
        Game.GetInstance().StartSpawnObjects();
    }

    private void OnDestroy()
    {
        Game.GetInstance().SpaceObjectSpawnEvent -= SpawnObject;
        Game.GetInstance().StopGame();
    }

    private void SpawnObject(object sender, SpaceObjectSpawnEventArgs e)
    {
        switch (e.ObjectType)
        {
            case SpaceObjectTypes.Asteroid:
                if (asteroid != null)
                    spaceObjectFactory.Create(asteroid, asteroidSpeed);
                break;
            case SpaceObjectTypes.Ufo:
                if (ufo != null)
                    spaceObjectFactory.Create(ufo);
                break;
            default:
                break;
        }
    }

    private void DestroyObjectsOfTag(string tag)
    {
        var objects = GameObject.FindGameObjectsWithTag(tag);
        foreach (var obj in objects)
        {
            Destroy(obj);
        }
    }

    public void UpdateScore(object sender, SpaceObjectDestroyedEventArgs e)
    {
        if (e != null)
            score += e.ScoresForDestroy;
        scorePanel.text = "Score: " + score;
    }

    private void UpdateLaserAccumulatorDisplay(int charge, int maxCharge)
    {
        if (laserAccumulatorCharge == null)
            return;

        laserAccumulatorCharge.localScale = new Vector2((float)charge / (float)maxCharge, 1.0f);
    }

    public void GameOver(object sender, SpaceObjectDestroyedEventArgs e)
    {
        Game.GetInstance().SpaceObjectSpawnEvent -= SpawnObject;
        Game.GetInstance().StopGame();
        if (menu != null)
            menu.SetActive(true);
    }

    public void RestartGame()
    {
        DestroyObjectsOfTag("Player");
        DestroyObjectsOfTag("Space object");
        DestroyObjectsOfTag("Weapon");
        Game.GetInstance().SpaceObjectSpawnEvent -= SpawnObject;
        Game.GetInstance().StopGame();
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
