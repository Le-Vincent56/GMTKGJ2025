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
            // Notify that a turn has started
            EventBus<StartTurn>.Raise(new StartTurn()
            {
            
            });
            
            turnController.StateFinished = State.Start;
        }
    }
}
