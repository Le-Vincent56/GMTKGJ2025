using Perennial.Core.Architecture.Event_Bus;
using Perennial.Core.Architecture.Event_Bus.Events;
using UnityEngine;

namespace Perennial.TurnController.States
{
    public class EndTurnState : BaseState
    {

        public EndTurnState(TurnController turnController) : base(turnController)
        {
        }

        public override void OnEnter()
        {
            Debug.Log($"{this} started");
            
            // Notify that a turn has started
            EventBus<TurnEnded>.Raise(new TurnEnded()
            {

            });

            turnController.StateFinished = State.End;
        }
    }
}
