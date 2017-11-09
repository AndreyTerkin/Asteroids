using UnityEngine;

namespace Assets.Scripts
{
    public class ScoreSubscriber
    {
        // In case of a big amount of objects on the scene this method can be slow
        // But under existing conditions it's acceptable
        public static void SubscribeScoreCounter(GameObject gameObject)
        {
            if (gameObject == null)
                return;

            GameObject gameControllerObject = GameObject.FindGameObjectWithTag("GameController");
            if (gameControllerObject == null)
                return;

            GameController gameController = gameControllerObject.GetComponent<GameController>();
            if (gameController == null)
                return;

            SpaceObject spaceObject = gameObject.GetComponent<SpaceObject>();
            spaceObject.SpaceObjectDestroyedEvent += gameController.UpdateScore;
        }
    }
}
