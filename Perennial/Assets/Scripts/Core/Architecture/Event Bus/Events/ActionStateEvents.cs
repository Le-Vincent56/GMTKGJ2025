using UnityEngine;

namespace Perennial.Core.Architecture.Event_Bus.Events
{
    public enum StateType
    {
        Harvest,
        Plant,
        Till,
        Nothing
    }
    public struct ExitActionState : IEvent
    {
        public StateType StateType;
    }

    public struct StartHarvestState : IEvent
    {
        
    }
    public struct StartPlantState : IEvent
    {
        
    }
    public struct StartTillState : IEvent
    {
        
    }
    public struct StartNothingState : IEvent
    {
        
    }
}
