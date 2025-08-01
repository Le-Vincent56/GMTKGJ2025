using Perennial.Core.Architecture.Event_Bus;
using Perennial.Core.Architecture.Event_Bus.Events;

namespace Perennial.TurnController.States.ActionStates
{
    public class HarvestActionState : BaseActionState
    {
        public override void OnEnter()
        {
            EventBus<StartHarvestState>.Raise(new StartHarvestState());
        }

        public override void OnExit()
        {
            
        }
    }
}
