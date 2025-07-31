using Perennial.Core.Architecture.State_Machine;
using UnityEngine;

namespace Perennial.TurnController.States
{
    /// <summary>
    /// Base state for other states to inherit from
    /// </summary>
    public class BaseState : IState
    {
        protected readonly TurnController turnController;
        public BaseState(TurnController turnController)
        {
            this.turnController = turnController;
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