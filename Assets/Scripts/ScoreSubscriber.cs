using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class ScoreSubscriber : MonoBehaviour
    {
        public static void SubscribePlayerToObjectEvent(GameObject gameObject)
        {
            if (gameObject)
            {
                Player player = FindObjectOfType<Player>();
                if (player == null)
                    return;

                SpaceObject spaceObject = gameObject.GetComponent<SpaceObject>();
                spaceObject.SpaceObjectDestroyedEvent += player.IncreaseScore;
            }
        }
    }
}
