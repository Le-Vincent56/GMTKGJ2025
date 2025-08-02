using Perennial.Core.Architecture.Event_Bus;
using Perennial.Core.Architecture.Event_Bus.Events;
using UnityEngine;

namespace Perennial.TurnManagement.States
{
    public class EndTurnState : BaseState
    {

        public EndTurnState(TurnController turnController) : base(turnController)
        {
        }

        public override void OnEnter()
        {
            Debug.Log($"{this} started");
            
            //reset end turn
            turnController.EndTurn = false;
            
            // Notify that a turn has started
            EventBus<TurnEnded>.Raise(new TurnEnded()
            {

            });
            
            turnController.StateFinished = State.End;
        }
    }
}
