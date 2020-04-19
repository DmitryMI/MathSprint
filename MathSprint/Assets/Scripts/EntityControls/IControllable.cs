namespace Assets.Scripts.EntityControls
{
    /// <summary>
    /// Interface for all entities that can handle movement commands
    /// </summary>
    public interface IControllable
    {
        /// <summary>
        /// Callback for movement commands
        /// </summary>
        /// <param name="horizontal">Horizontal movement [-1, 1]</param>
        /// <param name="vertical">Vertical movement [-1, 1]</param>
        /// <param name="jump">Jump [0, 1]</param>
        void OnControlInput(float horizontal, float vertical, float jump);
    }
}