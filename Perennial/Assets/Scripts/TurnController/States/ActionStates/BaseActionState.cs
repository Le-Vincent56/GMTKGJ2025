using Perennial.Core.Architecture.State_Machine;

namespace Perennial.TurnController.States.ActionStates
{
    /// <summary>
    /// Base state for other states to inherit from
    /// </summary>
    public class BaseActionState : IState
    {
        protected  BaseActionState()
        {
        }

        public virtual void OnEnter()
        {
        }

        public virtual void Update()
        {
        }

        public virtual void FixedUpdate()
        {
        }

        public virtual void OnExit()
        {
        }
    }
}