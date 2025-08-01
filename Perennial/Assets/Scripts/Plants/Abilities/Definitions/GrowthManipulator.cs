using System.Collections.Generic;
using Perennial.Plants.Behaviors;
using Perennial.Plants.Data;
using Perennial.Plants.Stats;
using UnityEngine;

namespace Perennial.Plants.Abilities.Definitions
{
    [CreateAssetMenu(fileName = "Growth Manipulator", menuName = "Plants/Abilities/Growth Manipulator")]
    public class GrowthManipulator : PassivePlantAbility
    {
        [Header("Growth Settings")]
        [SerializeField] private float growthMultiplier = 1.5f;
        
        public override void OnTick(PlantAbilityContext context)
        {
            // Get the list of affected plants
            List<Plant> affectedPlants = context.GardenManager.GetSurroundingPlants(
                context.OriginTile.GardenPosition.x,
                context.OriginTile.GardenPosition.y,
                effectRadius
            );

            // Iterate through each plant
            foreach (Plant plant in affectedPlants)
            {
                // Skip if the plant can have its growth manipulated
                if (plant.HasBehavior<GrowthManipulableBehaviorInstance>()) continue;
                
                // Send an affector out to the plant to have add a stat modifier
                plant.SendSignal(PlantSignal.Grow, new Affector(context.Plant.ID, StatType.GrowthRate,  growthMultiplier));
            }
        }

        /// <summary>
        /// Cancel the growth manipulation affects on other plants
        /// </summary>
        public override void Cancel(PlantAbilityContext context)
        {
            // Get the list of affected plants
            List<Plant> affectedPlants = context.GardenManager.GetSurroundingPlants(
                context.OriginTile.GardenPosition.x,
                context.OriginTile.GardenPosition.y,
                effectRadius
            );
            
            // Iterate through each plant
            foreach (Plant plant in affectedPlants)
            {
                // Skip if the plant can have its growth manipulated
                if (plant.HasBehavior<GrowthManipulableBehaviorInstance>()) continue;
                
                // Remove any modifiers relating to the context's Plant ID
                plant.Stats.Mediator.RemoveModifier(context.Plant.ID);
            }
        }
    }
}