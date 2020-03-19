using System;
using UnityEngine;

namespace Assets.Scripts.EntityControls.Mob
{
    public interface ICollisionEnterReporter
    {
        event Action<Collision2D> CollisionEnterEvent;
    }
}