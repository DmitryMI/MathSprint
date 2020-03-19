namespace Assets.Scripts.EntityControls
{
    public interface IControllable
    {
        void OnPlayerInput(float horizontal, float vertical, float jump);
    }
}