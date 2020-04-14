using System;
using UnityEngine;

namespace Assets.Scripts.EntityControls.Mob
{
    /// <summary>
    /// Represents object that can report a collision
    /// </summary>
    public interface ICollisionEnterReporter
    {
        /// <summary>
        /// Collision event
        /// </summary>
        event Action<Collision2D> CollisionEnterEvent;
    }
}