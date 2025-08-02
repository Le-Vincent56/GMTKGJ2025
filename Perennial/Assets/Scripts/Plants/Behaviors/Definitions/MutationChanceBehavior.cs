using Perennial.Plants.Data;
using Perennial.Plants.Stats;
using Perennial.Plants.Stats.Operations;
using UnityEngine;

namespace Perennial.Plants.Behaviors.Definitions
{
    [CreateAssetMenu(fileName = "Mutation Chance Behavior", menuName = "Plants/Behaviors/Mutation Chance")]
    public class MutationChanceBehavior : PlantBehavior
    {
        public override PlantBehaviorInstance CreateInstance(Plant plant)
        {
            return new MutationChanceBehaviorInstance(plant, this);
        }
    }

    public class MutationChanceBehaviorInstance : PlantBehaviorInstance
    {
        public MutationChanceBehaviorInstance(Plant owner, PlantBehavior definition)
            : base(owner, definition)
        { }
        
        public override bool HandleSignal(PlantSignal signalType, object data)
        {
            // Exit if the correct signal is not sent
            if (signalType != PlantSignal.MutationChance) return false;
            
            // Exit if the correct data is not sent
            if (data is not Affector affector) return false;
            
            // Exit if the correct stat type is not sent
            if(affector.Type is not StatType.MutationChance) return false;
            
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
