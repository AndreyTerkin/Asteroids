using UnityEngine;

namespace Assets.Scripts.Movers
{
    class MoverRelativeConstantAim : IMovable
    {
        private float speed;
        private Vector3 aimPosition;

        /// <summary>
        /// Create instance of MoverRelativeAim class
        /// </summary>
        /// <param name="aim">Space object for relative movement of mover</param>
        /// <param name="speed">Movement speed. Positive means movement towards aim, negative - away from aim</param>
        public MoverRelativeConstantAim(SpaceObject aim, float speed)
        {
            this.speed = speed;

            if (aim)
                aim.PositionChangedEvent += ChangeDirection;
        }

        public Vector3 UpdatePosition(Vector3 pos)
        {
            return Vector3.MoveTowards(pos,
                                       aimPosition,
                                       speed * Time.deltaTime);
        }

        private void ChangeDirection(Vector3 position)
        {
            aimPosition = position;
        }
    }
}
