public interface IState
{
    void Start();
    void Update();
    void HandleUpdate();
    void PhysicsUpdate();
    void End();
}