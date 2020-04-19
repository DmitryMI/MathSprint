

using UnityEngine;

namespace Assets.Scripts.EntityControls.Mob
{
    /// <summary>
    /// Represents a mob, that can walk towards specific point
    /// </summary>
    public interface IPointControllable
    {
        void OnControlInput(Vector2 targetPoint);
        Vector2 CurrentPosition { get; }
    }
}