using Perennial.Core.Architecture.Event_Bus;
using Perennial.Core.Architecture.Event_Bus.Events;
using UnityEngine;

namespace Perennial.TurnManagement.States.ActionStates
{
   public class TillActionState : BaseActionState
   {
      public override void OnEnter()
      {
         EventBus<StartTillState>.Raise(new StartTillState());
      }
   }
}