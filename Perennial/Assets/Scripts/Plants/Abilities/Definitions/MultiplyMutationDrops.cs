using System.Collections.Generic;
using Perennial.Plants.Behaviors;
using Perennial.Plants.Behaviors.Definitions;
using Perennial.Plants.Data;
using Perennial.Plants.Stats;
using Perennial.VFX;
using UnityEngine;

namespace Perennial.Plants.Abilities.Definitions
{
    [CreateAssetMenu(fileName = "Multiply Mutation Drops", menuName = "Plants/Abilities/Multiply Mutation Drops")]
    public class MultiplyMutationDrops : PassivePlantAbility
    {
        [Header("Mutation Drop Rate Settings")]
        [SerializeField] private float multiplier;
        
        public override bool CanExecute(PlantAbilityContext context)
        {
            // Only active while harvestable
            return onHarvest && context.Plant.Lifetime.FullyGrown;
        }

        public override void OnTick(PlantAbilityContext context)
        {
            // Get adjacent plants
            List<Plant> adjacentPlants = context.GardenManager.GetSurroundingPlants(
                context.OriginTile.GardenPosition.x,
                context.OriginTile.GardenPosition.y,
                effectRadius
            );
            
            // Mark adjacent pairs for double mutation
            foreach (Plant plant in adjacentPlants)
            {
                // Skip if the plant can have its growth manipulated
                if (!plant.HasBehavior<MutationDropRateBehaviorInstance>()) continue;
                
                plant.SendSignal(PlantSignal.MutationDropRate, new Affector()
                {
                    ID = context.Plant.ID,
                    Type = StatType.MutationDropRate,
                    Value = multiplier,
                });
                
                // Apply VFX
                VFXManager.Instance.AddVFX(plant.Tile, VFXType.Pearl, context.Plant.ID, true);
            }
        }

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
                // Skip if the plant can't have its growth manipulated
                if (!plant.HasBehavior<MutationDropRateBehaviorInstance>()) continue;
                
                // Remove any modifiers relating to the context's Plant ID
                plant.Stats.Mediator.RemoveModifier(context.Plant.ID);
                
                // Remove VFX
                VFXManager.Instance.RemoveVFX(plant.Tile, VFXType.Pearl, context.Plant.ID);
            }
        }
    }
}
