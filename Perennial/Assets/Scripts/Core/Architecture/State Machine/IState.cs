namespace Perennial.Core.Architecture.State_Machine
{
    /// <summary>
    /// Interface for a State, which will have effects on Enter,
    /// Exit, Update, and FixedUpdate
    /// </summary>
    public interface IState
    {
        void OnEnter();
        void Update();
        void FixedUpdate();
        void OnExit();
    }
}