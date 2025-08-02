using Perennial.Plants.Abilities;
using Perennial.Plants.Data;
using Perennial.Plants.Stats;
using Perennial.Plants.Stats.Operations;
using UnityEngine;

namespace Perennial.Plants.Behaviors
{
    [CreateAssetMenu(fileName = "Growth Manipulable Behavior", menuName="Plants/Behaviors/Growth Manipulable")]
    public class GrowthManipulableBehavior : PlantBehavior
    {
        public override PlantBehaviorInstance CreateInstance(Plant plant)
        {
            return new GrowthManipulableBehaviorInstance(plant, this);
        }
    }
    
    public class GrowthManipulableBehaviorInstance : PlantBehaviorInstance
    {

        public GrowthManipulableBehaviorInstance(Plant owner, PlantBehavior definition)
            : base(owner, definition)
        { }

        public override bool HandleSignal(PlantSignal signalType, object data)
        {
            // Exit if the correct signal is not sent
            if (signalType is not PlantSignal.Grow) return false;
            
            // Exit if the correct data is not sent
            if(data is not Affector affector) return false;
            
            // Exit if the correct stat type is not sent
            if(affector.Type is not StatType.GrowthRate) return false;

            // Exit if the mediator of the given ID already exists (prevents stacking the same modifier)
            if (owner.Stats.Mediator.ContainsModifierID(affector.ID)) return false;
            
            // Add the modifier to the Stats Mediator
            owner.Stats.Mediator.AddModifier(
                new StatModifier(
                    affector.ID,
                    affector.Type,
                    new MultiplyOperation(affector.Value)
                )
            );

            return true;
        }
    }
}