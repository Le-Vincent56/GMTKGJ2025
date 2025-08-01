using Perennial.Core.Architecture.Event_Bus;
using Perennial.Core.Architecture.Event_Bus.Events;
using UnityEngine;

namespace Perennial.TurnManagement.States.ActionStates
{
    public class HarvestActionState : BaseActionState
    {
        public override void OnEnter()
        {
            Debug.Log("Harvest State Started");
            EventBus<StartHarvestState>.Raise(new StartHarvestState());
        }
    }
}
