using JetBrains.Annotations;
using Perennial.Plants;

namespace Perennial.Core.Architecture.Event_Bus.Events
{
    public enum ActionStateType
    {
        Harvest,
        Plant,
        Till,
        Nothing
    }
    public struct ChangeActionState : IEvent
    {
        public ActionStateType StateType;
        [CanBeNull] public PlantDefinition SelectedPlantDefinition; 
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
