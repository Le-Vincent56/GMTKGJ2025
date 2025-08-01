using Perennial.Core.Architecture.Event_Bus;
using Perennial.Core.Architecture.Event_Bus.Events;
using UnityEngine;

namespace Perennial.TurnManagement.States.ActionStates
{
    public class PlantActionState : BaseActionState
    {
        public override void OnEnter()
        {
            Debug.Log("Plant State Started");
            EventBus<StartPlantState>.Raise(new StartPlantState());
        }
    }
}
