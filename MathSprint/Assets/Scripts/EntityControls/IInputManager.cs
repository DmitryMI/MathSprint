namespace Assets.Scripts.EntityControls
{
    /// <summary>
    /// Abstraction for movement commands source
    /// </summary>
    public interface IInputManager
    {
        /// <summary>
        /// Active IControllable
        /// </summary>
        IControllable PlayerControllable { get; set; }
    }
}