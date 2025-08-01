using Perennial.Core.Architecture.Event_Bus;
using Perennial.Core.Architecture.Event_Bus.Events;

namespace Perennial.TurnController.States.ActionStates
{
    public class NothingActionState : BaseActionState
    {
        public override void OnEnter()
        {
            EventBus<StartNothingState>.Raise(new StartNothingState());
        }
    }
}