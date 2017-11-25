using UnityEngine;
using AsteroidsLibrary.SpaceObjects;

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

            Player player = gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.LaserChargeChangedEvent += gameController.UpdateLaserAccumulatorDisplay;
                player.SpaceObject.SpaceObjectDestroyedEvent += gameController.GameOver;
                return;
            }

            ISpaceObject spaceObject = gameObject.GetComponent<ISpaceObject>();
            if (spaceObject != null)
                spaceObject.SpaceObject.SpaceObjectDestroyedEvent += gameController.UpdateScore;
        }
    }
}
