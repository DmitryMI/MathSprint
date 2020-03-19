using UnityEngine;

namespace Assets.Scripts.EntityControls.Mob
{
    public interface ICollisionMob : ICollisionEnterReporter, IControllable
    {
        Collider2D Collider2D { get; }
    }
}