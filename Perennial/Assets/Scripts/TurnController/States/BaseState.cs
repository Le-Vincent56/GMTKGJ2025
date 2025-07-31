using Perennial.Core.Architecture.State_Machine;
using UnityEngine;

namespace Perennial.TurnController.States
{
    /// <summary>
    /// Base state for other states to inherit from
    /// </summary>
    public class BaseState : IState
    {
        public BaseState()
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