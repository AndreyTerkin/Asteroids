using UnityEngine;

using AsteroidsLibrary;
using AsteroidsLibrary.SpaceObjects;

namespace Assets.Scripts
{
    class SceneObjectSpawner : Object
    {
        private GameObject asteroid;
        private GameObject asteroidFragment;
        private GameObject ufo;

        public SceneObjectSpawner(GameObject asteroid, GameObject asteroidFragment, GameObject ufo)
        {
            this.asteroid = asteroid;
            this.asteroidFragment = asteroidFragment;
            this.ufo = ufo;
        }

        public void SpawnObject(SpaceObjectSpawnEventArgs e)
        {
            switch (e.Object.Type)
            {
                case SpaceObjectTypes.Asteroid:
                    if (asteroid != null)
                    {
                        Create(
                            asteroid,
                            e.Object,
                            e.Position,
                            e.Object.Attributes.Speed,
                            e.Direction);
                    }
                    break;
                case SpaceObjectTypes.AsteroidFragment:
                    if (asteroidFragment != null)
                        Create(
                            asteroidFragment,
                            e.Object,
                            e.Position,
                            e.Object.Attributes.Speed,
                            e.Direction);
                    break;
                case SpaceObjectTypes.Ufo:
                    if (ufo != null)
                        Create(ufo, e.Object, e.Position);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Instantiate gameObject clone behind the border with given speed
        /// </summary>
        /// <param name="gameObject">GameObject instance to clone</param>
        /// <param name="position"></param>
        /// <param name="direction"></param>
        /// <param name="speed"></param>
        public static void Create(GameObject gameObject, SpaceObject spaceObject, Vector3 position, float speed, Vector2 direction)
        {
            if (gameObject == null)
                return;

            GameObject clone = Instantiate(gameObject, position, Quaternion.identity);
            Rigidbody2D rb = clone.GetComponent<Rigidbody2D>();
            rb.AddForce(direction.normalized * speed, ForceMode2D.Impulse);
            var obj = clone.GetComponent<ISpaceObject>();
            obj.SpaceObject = spaceObject;
            ScoreSubscriber.SubscribeScoreCounter(clone);
        }

        /// <summary>
        /// Instantiate gameObject clone behind the border without impulse
        /// </summary>
        /// <param name="gameObject">GameObject instance to clone</param>
        /// <param name="position"></param>
        public static void Create(GameObject gameObject, SpaceObject spaceObject, Vector3 position)
        {
            if (gameObject == null)
                return;

            GameObject clone = Instantiate(gameObject, position, Quaternion.identity);
            var obj = clone.GetComponent<ISpaceObject>();
            obj.SpaceObject = spaceObject;
            ScoreSubscriber.SubscribeScoreCounter(clone);
        }
    }
}
