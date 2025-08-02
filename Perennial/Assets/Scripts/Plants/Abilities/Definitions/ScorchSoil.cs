using System.Collections.Generic;
using Perennial.Garden;
using Perennial.Plants.Behaviors;
using Perennial.Plants.Data;
using Perennial.Plants.Stats;
using UnityEngine;

namespace Perennial.Plants.Abilities.Definitions
{
    [CreateAssetMenu(fileName = "Scorch Soil", menuName = "Plants/Abilities/Scorch Soil")]
    public class ScorchSoil : PlacePlantAbility
    {
        [Header("Scorch Settings")] 
        [SerializeField] private int foodBonus = 10;
        [SerializeField] private float decayRateMultiplier = 0.5f;

        public override void OnPlace(PlantAbilityContext context)
        {
            // Get the surrounding tiles
            List<Tile> affectedTiles = context.GardenManager.GetSurroundingTiles(
                context.OriginTile,
                effectRadius
            );

            // Apply scorch effect to each tile
            foreach (Tile tile in affectedTiles)
            {
                // Send signal to apply scorch effect
                if (tile.HasPlant)
                {
                    tile.Plant.SendSignal(PlantSignal.Scorch, new Affector()
                    { 
                        ID = context.Plant.ID,
                        Type = StatType.FoodModifier,
                        Value = foodBonus,
                    });
                }
                
                // TODO: Add a scorch modifier to the tile
            }
        }
    }
}
