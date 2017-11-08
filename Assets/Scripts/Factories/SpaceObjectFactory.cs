using UnityEngine;

namespace Assets.Scripts.Factories
{
    class SpaceObjectFactory : MonoBehaviour
    {
        protected Transform _transform;
        protected Border _border;

        private static float _errorOffset = 0.25f;

        public SpaceObjectFactory(Transform transform, Border border)
        {
            _transform = transform;
            _border = border;
        }

        public GameObject Create(GameObject gameObject, ref Vector2 direction)
        {
            BoxCollider2D collider = gameObject.GetComponent<BoxCollider2D>();
            Vector3 position = new Vector3();
            InitSpawnParameters(collider, ref position, ref direction);
            return Instantiate(gameObject, position, Quaternion.identity) as GameObject;
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

            Debug.Log("Collider width: " + collider.size.x);
            Debug.Log("Spawning: " + _border.borderXmax + " " + _border.borderXmin);
        }
    }
}
