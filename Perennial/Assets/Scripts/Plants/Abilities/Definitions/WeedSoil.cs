using System.Collections.Generic;
using Perennial.Garden;
using Perennial.VFX;
using UnityEngine;

namespace Perennial.Plants.Abilities.Definitions
{
    [CreateAssetMenu(fileName = "Weed Soil", menuName = "Plants/Abilities/Weed Soil")]
    public class WeedSoil : HarvestPlantAbility
    {
        public override void OnHarvest(PlantAbilityContext context)
        {
            // Get the surrounding tiles
            List<Tile> affectedTiles = context.GardenManager.GetSurroundingTiles(
                context.OriginTile,
                effectRadius
            );

            // Iterate through each affected tile
            foreach (Tile tile in affectedTiles)
            {
                // Set the soil state to Tilled
                tile.SoilState = SoilState.WEEDS;
                
                // Apply VFX
                VFXManager.Instance.AddVFX(tile, VFXType.Carrot, context.Plant.ID, false);
            }
        }
    }
}
