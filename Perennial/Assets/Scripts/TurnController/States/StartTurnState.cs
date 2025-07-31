using Perennial.Core.Architecture.Event_Bus;
using Perennial.Core.Architecture.Event_Bus.Events;

namespace Perennial.TurnController.States
{
    public class StartTurnState : BaseState
    {
        
        public StartTurnState (TurnController turnController) : base(turnController)
        {
        }
        public override void OnEnter()
        {
            //reset actions
            turnController.ActionsTaken = 0;
            
            // Notify that a turn has started
            EventBus<TurnStarted>.Raise(new TurnStarted()
            {
            
            });
            
            turnController.StateFinished = State.Start;
        }
    }
}
