namespace Assets.Scripts.EntityControls
{
    public interface IControllable
    {
        void OnControlInput(float horizontal, float vertical, float jump);
    }
}