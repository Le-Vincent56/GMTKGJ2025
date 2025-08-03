using System.Collections.Generic;
using Perennial.Plants.Behaviors;
using UnityEngine;

namespace Perennial.Plants.Abilities.Definitions
{
    [CreateAssetMenu(fileName = "Refresh Harvest Window", menuName = "Plants/Abilities/Refresh Harvest Window")]
    public class RefreshHarvestWindow : HarvestPlantAbility
    {
        public override void OnHarvest(PlantAbilityContext context)
        {
            List<Plant> affectedPlants = context.GardenManager.GetSurroundingPlants(
                context.OriginTile.GardenPosition.x,
                context.OriginTile.GardenPosition.y,
                effectRadius
            );

            foreach (Plant plant in affectedPlants)
            {
                plant.SendSignal(PlantSignal.RefreshHarvest, null);
            }
        }
    }
}
