using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Movers
{
    interface IMovable
    {
        Vector3 UpdatePosition(Vector3 currentPosition);
    }
}
