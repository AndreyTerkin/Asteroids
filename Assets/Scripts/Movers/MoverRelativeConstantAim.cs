using UnityEngine;

namespace Assets.Scripts.Movers
{
    class MoverRelativeConstantAim : IMovable
    {
        private float speed;
        private Vector3 aimPosition;

        /// <summary>
        /// Create instance of MoverRelativeConstantAim class
        /// </summary>
        /// <param name="aim">Space object for relative movement of mover</param>
        /// <param name="speed">Movement speed. Positive means movement towards aim, negative - away from aim</param>
        public MoverRelativeConstantAim(SpaceObject aim, float speed)
        {
            this.speed = speed;

            if (aim)
                aim.PositionChangedEvent += ChangeDirection;
        }

        public Vector3 UpdatePosition(Vector3 currentPosition)
        {
            return Vector3.MoveTowards(currentPosition,
                                       aimPosition,
                                       speed * Time.deltaTime);
        }

        private void ChangeDirection(Vector3 endPosition)
        {
            aimPosition = endPosition;
        }
    }
}
