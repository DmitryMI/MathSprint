

using UnityEngine;

namespace Assets.Scripts.EntityControls.Mob
{
    public interface IPointControllable
    {
        void OnControlInput(Vector2 targetPoint);
        Vector2 CurrentPosition { get; }
    }
}