using UnityEngine;

namespace Assets.Scripts.Factories
{
    class SpaceObjectFactory : Object
    {
        protected Transform _transform;
        protected Border _border;

        private static float _errorOffset = 0.25f;

        public SpaceObjectFactory(Transform transform, Border border)
        {
            _transform = transform;
            _border = border;
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
            rb.AddForce(direction * speed, ForceMode2D.Impulse);

            ScoreSubscriber.SubscribeScoreCounter(clone);
        }

        /// <summary>
        /// Instantiate gameObject clone with given position, direction and speed
        /// </summary>
        /// <param name="gameObject">GameObject instance to clone</param>
        /// <param name="position"></param>
        /// <param name="direction"></param>
        /// <param name="speed"></param>
        public static void Create(GameObject gameObject, Vector3 position, Vector2 direction, float speed)
        {
            if (gameObject == null)
                return;

            GameObject clone = Instantiate(gameObject, position, Quaternion.identity);
            Rigidbody2D rb = clone.GetComponent<Rigidbody2D>();
            rb.AddForce(direction * speed, ForceMode2D.Impulse);

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

            BoxCollider2D collider = gameObject.GetComponent<BoxCollider2D>();
            Vector3 position = new Vector3();
            Vector2 direction = new Vector2(1.0f, 1.0f);

            InitSpawnParameters(collider, ref position, ref direction);
            GameObject clone = Instantiate(gameObject, position, Quaternion.identity);
            Rigidbody2D rb = clone.GetComponent<Rigidbody2D>();
            rb.AddForce(direction * speed, ForceMode2D.Impulse);

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

            BoxCollider2D collider = gameObject.GetComponent<BoxCollider2D>();
            Vector3 position = new Vector3();
            Vector2 direction = new Vector2(1.0f, 1.0f);

            InitSpawnParameters(collider, ref position, ref direction);
            GameObject clone = Instantiate(gameObject, position, Quaternion.identity);

            ScoreSubscriber.SubscribeScoreCounter(clone);
        }

        protected virtual void InitSpawnParameters(BoxCollider2D collider, ref Vector3 position, ref Vector2 direction)
        {
            float deflection = Random.Range(-1.0f, 1.0f);
            float side = Random.Range(0.0f, 4.0f);

            if (side >= 0 && side < 1) // top
            {
                position = new Vector3(Random.Range(-_border.borderXmax, _border.borderXmax),
                                       _border.borderYmax + collider.size.y,
                                       0.0f);
                direction = -_transform.up + new Vector3(deflection, 0.0f);
            }
            else if (side >= 1 && side < 2) // bottom
            {
                position = new Vector3(Random.Range(-_border.borderXmax, _border.borderXmax),
                                       _border.borderYmin - collider.size.y - _errorOffset,
                                       0.0f);
                direction = _transform.up + new Vector3(deflection, 0.0f);
            }
            else if (side >= 2 && side < 3) // right
            {
                position = new Vector3(_border.borderXmax + collider.size.x,
                                       Random.Range(-_border.borderYmax, _border.borderYmax),
                                       0.0f);
                direction = -_transform.right + new Vector3(0.0f, deflection);
            }
            else if (side >= 3 && side < 4) // left
            {
                position = new Vector3(_border.borderXmin - collider.size.x,
                                       Random.Range(-_border.borderYmax, _border.borderYmax),
                                       0.0f);
                direction = _transform.right + new Vector3(0.0f, deflection);
            }
        }
    }
}
