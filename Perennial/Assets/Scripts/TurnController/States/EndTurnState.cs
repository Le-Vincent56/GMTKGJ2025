using Perennial.Core.Architecture.Event_Bus;
using Perennial.Core.Architecture.Event_Bus.Events;

namespace Perennial.TurnController.States
{
    public class EndTurnState : BaseState
    {
        
        public EndTurnState (TurnController turnController) : base(turnController)
        {
        }
        public override void OnEnter()
        {
            // Notify that a turn has started
            EventBus<EndTurn>.Raise(new EndTurn()
            {
            
            });
            
            turnController.
        }
    }
}
