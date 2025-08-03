using System.Collections.Generic;
using Perennial.Garden;
using Perennial.Plants.Behaviors;
using Perennial.Plants.Behaviors.Definitions;
using Perennial.Plants.Data;
using Perennial.Plants.Stats;
using Perennial.VFX;
using UnityEngine;

namespace Perennial.Plants.Abilities.Definitions
{
    [CreateAssetMenu(fileName = "Mutation Chance", menuName = "Plants/Abilities/Mutation Chance")]
    public class MutationAndDecayManipulator : PassivePlantAbility
    {
        [Header("Effect Settings")]
        [SerializeField] private float mutationChanceBonus = 0.1f; // +10% mutation chance
        [SerializeField] private int soilDecayDelay = 1;
        
        public override bool CanExecute(PlantAbilityContext context)
        {
            // Only active while growing (not fully grown yet)
            return onGrow && !context.Plant.Lifetime.FullyGrown;
        }
        
        public override void OnTick(PlantAbilityContext context)
        {
            // Get affected tiles for soil decay delay
            List<Tile> affectedTiles = context.GardenManager.GetSurroundingTiles(
                context.OriginTile,
                effectRadius
            );
            
            // Apply soil decay delay
            foreach (Tile tile in affectedTiles)
            {
                // Check if the tile has a plant
                if (tile.HasPlant)
                {
                    // Extract the plant
                    Plant plantOnTile = tile.Plant;
                    
                    // Skip if the plant can't have its mutation manipulated
                    if (plantOnTile.HasBehavior<MutationChanceBehaviorInstance>()) continue;
                    
                    // Add the mutation bonus to that plant
                    plantOnTile.SendSignal(PlantSignal.MutationChance, new Affector()
                    {
                        ID = context.Plant.ID,
                        Type = StatType.MutationChance,
                        Value = 1 + mutationChanceBonus,
                    });
                    
                    // Apply VFX
                    VFXManager.Instance.AddVFX(tile, VFXType.Ivy, context.Plant.ID, true);
                }
                
                // TODO: Apply soil decay delay
                //tile.SendSignal(TileSignal.DelayDecay, soilDecayDelay);
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
                // Skip if the plant can't have its mutation manipulated
                if (!plant.HasBehavior<MutationChanceBehaviorInstance>()) continue;
                
                // Remove any modifiers relating to the context's Plant ID
                plant.Stats.Mediator.RemoveModifier(context.Plant.ID);
                
                // Remove VFX
                VFXManager.Instance.RemoveVFX(plant.Tile, VFXType.Ivy, context.Plant.ID);
            }
        }
    }
}
