using UnityEngine;

using AsteroidsLibrary;
using AsteroidsLibrary.SpaceObjects;

namespace Assets.Scripts
{
    class SceneObjectSpawner : Object
    {
        private GameObject asteroid;
        private GameObject ufo;

        public SceneObjectSpawner(GameObject asteroid, GameObject ufo)
        {
            this.asteroid = asteroid;
            this.ufo = ufo;
        }

        public void SpawnObject(object sender, SpaceObjectSpawnEventArgs e)
        {
            switch (e.ObjectType)
            {
                case SpaceObjectTypes.Asteroid:
                    if (asteroid != null)
                    {
                        Create(
                            asteroid,
                            e.Position,
                            e.Direction,
                            e.Attributes.Speed);
                    }
                    break;
                case SpaceObjectTypes.Ufo:
                    if (ufo != null)
                        Create(ufo, e.Position);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Instantiate gameObject clone with random direction and given position and speed
        /// </summary>
        /// <param name="gameObject">GameObject instance to clone</param>
        /// <param name="position"></param>
        /// <param name="speed"></param>
        public static void Create(GameObject gameObject, Vector3 position, float speed)
        {
            if (gameObject == null)
                return;

            Vector2 direction = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
            GameObject clone = Instantiate(gameObject, position, Quaternion.identity);
            Rigidbody2D rb = clone.GetComponent<Rigidbody2D>();
            rb.AddForce(direction.normalized * speed, ForceMode2D.Impulse);
            ScoreSubscriber.SubscribeScoreCounter(clone);
        }

        /// <summary>
        /// Instantiate gameObject clone behind the border with given speed
        /// </summary>
        /// <param name="gameObject">GameObject instance to clone</param>
        /// <param name="speed"></param>
        public void Create(GameObject gameObject, Vector3 position, Vector2 direction, float speed)
        {
            if (gameObject == null)
                return;

            GameObject clone = Instantiate(gameObject, position, Quaternion.identity);
            Rigidbody2D rb = clone.GetComponent<Rigidbody2D>();
            rb.AddForce(direction.normalized * speed, ForceMode2D.Impulse);
            ScoreSubscriber.SubscribeScoreCounter(clone);
        }

        /// <summary>
        /// Instantiate gameObject clone behind the border without impulse
        /// </summary>
        /// <param name="gameObject">GameObject instance to clone</param>
        public void Create(GameObject gameObject, Vector3 position)
        {
            if (gameObject == null)
                return;

            GameObject clone = Instantiate(gameObject, position, Quaternion.identity);
            ScoreSubscriber.SubscribeScoreCounter(clone);
        }
    }
}
