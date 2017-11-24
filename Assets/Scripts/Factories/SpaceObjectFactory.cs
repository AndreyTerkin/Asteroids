using UnityEngine;

namespace Assets.Scripts.Factories
{
    class SpaceObjectFactory : Object
    {
        protected Transform transform;
        protected Border border;

        private AsteroidsLibrary.SpaceObjects.Factory.SpaceObjectFactory factory;

        public SpaceObjectFactory(Transform transform, Border border)
        {
            this.transform = transform;
            this.border = border;

            factory = new AsteroidsLibrary.SpaceObjects.Factory.SpaceObjectFactory(
                border.xMin, border.xMax,
                border.yMin, border.yMax);
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
        public void Create(GameObject gameObject, float speed)
        {
            if (gameObject == null)
                return;

            BoxCollider2D collider = GetCollider(gameObject);
            if (collider == null)
                return;

            Vector3 position = new Vector3();
            Vector2 direction = new Vector2(1.0f, 1.0f);

            factory.InitSpawnParameters(collider.size, ref position, ref direction);
            GameObject clone = Instantiate(gameObject, position, Quaternion.identity);
            Rigidbody2D rb = clone.GetComponent<Rigidbody2D>();
            rb.AddForce(direction.normalized * speed, ForceMode2D.Impulse);

            ScoreSubscriber.SubscribeScoreCounter(clone);
        }

        /// <summary>
        /// Instantiate gameObject clone behind the border without impulse
        /// </summary>
        /// <param name="gameObject">GameObject instance to clone</param>
        public void Create(GameObject gameObject)
        {
            if (gameObject == null)
                return;

            BoxCollider2D collider = GetCollider(gameObject);
            if (collider == null)
                return;

            Vector3 position = new Vector3();
            Vector2 direction = new Vector2(1.0f, 1.0f);

            factory.InitSpawnParameters(collider.size, ref position, ref direction);
            GameObject clone = Instantiate(gameObject, position, Quaternion.identity);

            ScoreSubscriber.SubscribeScoreCounter(clone);
        }

        private BoxCollider2D GetCollider(GameObject gameObj)
        {
            foreach (Transform child in gameObj.transform)
            {
                if(child.tag == "Representation")
                {
                    for (int i=0; i < child.childCount; i++)
                    {
                        if (child.GetChild(i).gameObject.activeSelf)
                            return child.GetChild(i).GetComponent<BoxCollider2D>();
                    }
                }
            }
            return null;
        }
    }
}
