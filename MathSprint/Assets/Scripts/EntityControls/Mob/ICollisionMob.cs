using UnityEngine;

namespace Assets.Scripts.EntityControls.Mob
{
    /// <summary>
    /// Represents a mob, that can collide surface
    /// </summary>
    public interface ICollisionMob : ICollisionEnterReporter, IControllable
    {
        Collider2D Collider2D { get; }
    }
}