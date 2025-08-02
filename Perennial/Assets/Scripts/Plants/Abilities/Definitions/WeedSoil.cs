using System.Collections.Generic;
using Perennial.Garden;
using UnityEngine;

namespace Perennial.Plants.Abilities.Definitions
{
    [CreateAssetMenu(fileName = "Weed Soil", menuName = "Plants/Abilities/Weed Soil")]
    public class WeedSoil : PlacePlantAbility
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
                tile.SoilState = SoilState.WEEDS;
            }
        }
    }
}
