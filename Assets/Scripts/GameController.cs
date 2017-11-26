using UnityEngine;
using UnityEngine.UI;

using Assets.Scripts;

using AsteroidsLibrary;
using AsteroidsLibrary.SpaceObjects;

public class GameController : MonoBehaviour
{
    public Text scorePanel;
    public GameObject menu;
    public RectTransform laserAccumulatorCharge;

    private Game gameInstance;
    private SceneObjectSpawner sceneObjectSpawner;
    private RepresentationManager representationManager;

    private Border border;
    private GameObject player;
    private GameObject asteroid;
    private GameObject asteroidFragment;
    private GameObject ufo;
    private GameObject bullet;
    private GameObject laser;

    private void Start()
    {
        gameInstance = Game.GetInstance();

        UpdateScore(0);

        representationManager = GetComponent<RepresentationManager>();
        player = representationManager.multiRepresentableObjects[1];
        ufo = representationManager.multiRepresentableObjects[2];
        asteroid = representationManager.multiRepresentableObjects[3];
        asteroidFragment = representationManager.multiRepresentableObjects[4];
        bullet = representationManager.multiRepresentableObjects[5];
        laser = representationManager.multiRepresentableObjects[6];

        if (menu != null)
            menu.SetActive(false);

        UpdateLaserAccumulatorDisplay(1, 1); // Reset value

        Boundary boundary = FindObjectOfType<Boundary>();
        if (boundary)
            border = boundary.border;

        sceneObjectSpawner = new SceneObjectSpawner(this, player, asteroid, asteroidFragment, ufo);
        ConfigureServer();
    }

    private void ConfigureServer()
    {
        gameInstance.MessageDelegateEvent += PrintServerMessage;
        gameInstance.SetBorders(border);

        AddObjectToServer(player, SpaceObjectTypes.Player);
        AddObjectToServer(asteroid, SpaceObjectTypes.Asteroid);
        AddObjectToServer(asteroidFragment, SpaceObjectTypes.AsteroidFragment);
        AddObjectToServer(ufo, SpaceObjectTypes.Ufo);
        AddObjectToServer(bullet, SpaceObjectTypes.Bullet);
        AddObjectToServer(laser, SpaceObjectTypes.Laser);

        ObjectSpawner.SpaceObjectSpawnEvent += sceneObjectSpawner.SpawnObject;
        gameInstance.ScoreUpdateEvent += UpdateScore;
        gameInstance.GameOverEvent += GameOver;
        gameInstance.StartSpawnObjects();
    }

    private void ResetServer()
    {
        gameInstance.MessageDelegateEvent -= PrintServerMessage;
        ObjectSpawner.SpaceObjectSpawnEvent -= sceneObjectSpawner.SpawnObject;
        gameInstance.ScoreUpdateEvent -= UpdateScore;
        gameInstance.GameOverEvent -= GameOver;
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

    public void UpdateScore(int score)
    {
        scorePanel.text = "Score: " + score;
    }

    public void UpdateLaserAccumulatorDisplay(int charge, int maxCharge)
    {
        if (laserAccumulatorCharge == null)
            return;

        laserAccumulatorCharge.localScale = new Vector2((float)charge / (float)maxCharge, 1.0f);
    }

    public void GameOver()
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

    private void AddObjectToServer(GameObject gameObject, SpaceObjectTypes type)
    {
        var collider = gameObject.GetComponent<BoxCollider2D>();
        Vector2 size;
        if (collider != null)
            size = collider.size;
        else
            size = Vector2.one;

        ISpaceObject spaceObject = gameObject.GetComponent<ISpaceObject>();
        gameInstance.AddUnit(type,
            size,
            spaceObject.Speed,
            spaceObject.ScoresForDestroy);
    }
}
