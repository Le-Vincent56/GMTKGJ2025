using Perennial.Core.Architecture.Event_Bus;
using Perennial.Core.Architecture.Event_Bus.Events;
using UnityEngine;

namespace Perennial.TurnManagement.States.ActionStates
{
    public class HarvestActionState : BaseActionState
    {
        public override void OnEnter()
        {
            EventBus<StartHarvestState>.Raise(new StartHarvestState());
        }
    }
}
