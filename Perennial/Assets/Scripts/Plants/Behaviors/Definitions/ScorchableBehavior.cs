using Perennial.Plants.Data;
using Perennial.Plants.Stats;
using Perennial.Plants.Stats.Operations;
using UnityEngine;

namespace Perennial.Plants.Behaviors.Definitions
{
    [CreateAssetMenu(fileName = "Scorchable Behavior", menuName = "Plants/Behaviors/Scorchable")]
    public class ScorchableBehavior : PlantBehavior
    {
        public override PlantBehaviorInstance CreateInstance(Plant plant)
        {
            return new ScorchableBehaviorInstance(plant, this);
        }
    }
    
    public class ScorchableBehaviorInstance : PlantBehaviorInstance
    {
        public ScorchableBehaviorInstance(Plant owner, PlantBehavior definition)
            : base(owner, definition)
        { }

        public override bool HandleSignal(PlantSignal signalType, object data)
        {
            // Exit if the correct signal is not sent
            if (signalType != PlantSignal.Scorch) return false;
            
            // Exit if the correct data is not sent
            if (data is not Affector affector) return false;
            
            // Exit if the correct stat type is not sent
            if(affector.Type is not StatType.FoodModifier) return false;
            
            // Add the modifier to the Stats Mediator
            owner.Stats.Mediator.AddModifier(
                new StatModifier(
                    affector.ID,
                    affector.Type,
                    new AddOperation(affector.Value)
                )
            );
            
            return true;
        }
    }
}
