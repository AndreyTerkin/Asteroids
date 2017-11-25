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

    private int score;

    private void Start()
    {
        gameInstance = Game.GetInstance();

        score = 0;
        UpdateScore(this, null);

        representationManager = GetComponent<RepresentationManager>();
        player = representationManager.multiRepresentableObjects[1];
        asteroid = representationManager.multiRepresentableObjects[3];
        asteroidFragment = representationManager.multiRepresentableObjects[4];
        ufo = representationManager.multiRepresentableObjects[2];
        //player = representationManager.GetGameObjectOfType<Player>();
        //asteroid = representationManager.GetGameObjectOfType<Asteroid>();
        //asteroidFragment = representationManager.GetGameObjectOfType<AsteroidFragment>();
        //ufo = representationManager.GetGameObjectOfType<Ufo>();

        if (menu != null)
            menu.SetActive(false);

        Boundary boundary = FindObjectOfType<Boundary>();
        if (boundary)
            border = boundary.border;

        UpdateLaserAccumulatorDisplay(1, 1); // Reset value

        sceneObjectSpawner = new SceneObjectSpawner(player, asteroid, asteroidFragment, ufo);
        ConfigureServer();
    }

    private void ConfigureServer()
    {
        gameInstance.MessageDelegateEvent += PrintServerMessage;
        gameInstance.SetBorders(border);

        Player playerScript = player.GetComponent<Player>();
        gameInstance.AddUnit(SpaceObjectTypes.Player,
            Vector2.one,
            playerScript.Speed,
            playerScript.ScoresForDestroy);

        var asteroidCollider = GetCollider(asteroid);
        EngineSpaceObject asteroidScript = asteroid.GetComponent<EngineSpaceObject>();
        gameInstance.AddUnit(SpaceObjectTypes.Asteroid,
            asteroidCollider.size,
            asteroidScript.Speed,
            asteroidScript.ScoresForDestroy);

        var asteroidFragmentCollider = GetCollider(asteroidFragment);
        EngineSpaceObject asteroidFragmentScript = asteroidFragment.GetComponent<EngineSpaceObject>();
        gameInstance.AddUnit(SpaceObjectTypes.AsteroidFragment,
            asteroidFragmentCollider.size,
            asteroidFragmentScript.Speed,
            asteroidFragmentScript.ScoresForDestroy);

        var ufoCollider = GetCollider(ufo);
        EngineSpaceObject ufoScript = ufo.GetComponent<EngineSpaceObject>();
        gameInstance.AddUnit(SpaceObjectTypes.Ufo,
            ufoCollider.size,
            ufoScript.Speed,
            ufoScript.ScoresForDestroy);

        ObjectSpawner.SpaceObjectSpawnEvent += sceneObjectSpawner.SpawnObject;
        gameInstance.StartSpawnObjects();
    }

    private void ResetServer()
    {
        ObjectSpawner.SpaceObjectSpawnEvent -= sceneObjectSpawner.SpawnObject;
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

    public void UpdateLaserAccumulatorDisplay(int charge, int maxCharge)
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
