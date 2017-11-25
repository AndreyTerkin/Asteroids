using UnityEngine;
using UnityEngine.UI;

using Assets.Scripts;

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

    private Game gameInstance;
    private SceneObjectSpawner sceneObjectSpawner;
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
        gameInstance = Game.GetInstance();

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

        sceneObjectSpawner = new SceneObjectSpawner(asteroid, ufo);
        ConfigureServer();
    }

    private void ConfigureServer()
    {
        gameInstance.MessageDelegateEvent += PrintServerMessage;
        gameInstance.SetBorders(border);

        var asteroidCollider = GetCollider(asteroid);
        Asteroid asteroidScript = asteroid.GetComponent<Asteroid>();
        gameInstance.AddUnit(SpaceObjectTypes.Asteroid,
            asteroidCollider.size,
            asteroidScript.speed,
            asteroidScript.scoreForDestroy);

        var ufoCollider = GetCollider(ufo);
        Ufo ufoScript = ufo.GetComponent<Ufo>();
        gameInstance.AddUnit(SpaceObjectTypes.Ufo,
            ufoCollider.size,
            ufoScript.speed,
            ufoScript.scoreForDestroy);

        gameInstance.SpaceObjectSpawnEvent += sceneObjectSpawner.SpawnObject;
        gameInstance.StartSpawnObjects();
    }

    private void ResetServer()
    {
        gameInstance.SpaceObjectSpawnEvent -= sceneObjectSpawner.SpawnObject;
        gameInstance.MessageDelegateEvent -= PrintServerMessage;
        gameInstance.StopGame();
    }

    private void OnDestroy()
    {
        ResetServer();
    }

    private void PrintServerMessage(string message)
    {
        Debug.Log(message);
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
        ResetServer();
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

    private BoxCollider2D GetCollider(GameObject gameObj)
    {
        foreach (Transform child in gameObj.transform)
        {
            if (child.tag == "Representation")
            {
                for (int i = 0; i < child.childCount; i++)
                {
                    if (child.GetChild(i).gameObject.activeSelf)
                        return child.GetChild(i).GetComponent<BoxCollider2D>();
                }
            }
        }
        return null;
    }
}
