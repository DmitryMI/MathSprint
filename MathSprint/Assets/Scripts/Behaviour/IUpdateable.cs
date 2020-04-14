namespace Assets.Scripts.Behaviour
{
    public interface IUpdateable
    {
        /// <summary>
        /// Will be invoked each frame by parent BehaviourManager
        /// </summary>
        void OnUpdate();
    }
}