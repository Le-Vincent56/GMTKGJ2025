using Perennial.Core.Architecture.Event_Bus;
using Perennial.Core.Architecture.Event_Bus.Events;
using UnityEngine;

namespace Perennial.TurnController.States.ActionStates
{
    public class NothingActionState : BaseActionState
    {
        public override void OnEnter()
        {
            Debug.Log("Nothing State Started");
            EventBus<StartNothingState>.Raise(new StartNothingState());
        }
    }
}