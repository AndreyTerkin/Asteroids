using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Movers
{
    class MoverRelativeNearestObject : IMovable
    {
        private Type aimType;
        private float speed;
        private Vector3 aimPosition;

        /// <summary>
        /// Create instance of MoverRelativeNearestObject class
        /// </summary>
        /// <param name="aimType">Object type for relative movement of mover. For correct work must be SpaceObject or subclass type</param>
        /// <param name="speed">Movement speed. Positive means movement towards aim, negative - away from aim</param>
        public MoverRelativeNearestObject(Type aimType, float speed)
        {
            // TODO: make hard contract on SpaceObject type
            this.aimType = aimType;
            this.speed = speed;
        }

        public Vector3 UpdatePosition(Vector3 currentPosition)
        {
            IEnumerable<SpaceObject> objects = GameObject.FindObjectsOfType(aimType).OfType<SpaceObject>();
            if (objects.Count() == 0)
                return currentPosition;

            float minDistance = float.MaxValue;
            SpaceObject nearestObject = null;
            foreach (var spaceObject in objects)
            {
                float distance = Vector3.Distance(currentPosition, spaceObject.transform.position);
                if (minDistance > distance)
                {
                    minDistance = distance;
                    nearestObject = spaceObject;
                }
            }

            if (nearestObject != null)
            {
                nearestObject.PositionChangedEvent += ChangeDirection;
            }

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
