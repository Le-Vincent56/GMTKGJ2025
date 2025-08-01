using Perennial.Core.Architecture.Event_Bus;
using Perennial.Core.Architecture.Event_Bus.Events;
using UnityEngine;

namespace Perennial.TurnController.States
{
    public class StartTurnState : BaseState
    {
        
        public StartTurnState (TurnController turnController) : base(turnController)
        {
        }
        public override void OnEnter()
        { 
            Debug.Log($"{this} started");
            
            //reset actions
            turnController.ResetActions();
            
            // Notify that a turn has started
            EventBus<TurnStarted>.Raise(new TurnStarted()
            {
                
            });

            turnController.StateFinished = State.Start;
        }
    }
}
