using System.Collections.Generic;
using Perennial.Garden;
using Perennial.VFX;
using UnityEngine;

namespace Perennial.Plants.Abilities.Definitions
{
    [CreateAssetMenu(fileName = "Till Soil", menuName = "Plants/Abilities/Till Soil")]
    public class TillSoil : PlacePlantAbility
    {
        public override void OnPlace(PlantAbilityContext context)
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
                tile.SoilState = SoilState.TILLED;
                
                // Apply VFX
                VFXManager.Instance.AddVFX(tile, VFXType.Garlic, context.Plant.ID, false);
            }
        }
    }
}
