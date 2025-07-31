using System.Collections.Generic;
using Perennial.Plants.Behaviors;
using Perennial.Plants.Data;
using UnityEngine;

namespace Perennial.Plants.Abilities.Definitions
{
    [CreateAssetMenu(fileName = "Freeze Nearby Plants", menuName = "Plants/Abilities/Freeze Nearby Plants")]
    public class FreezeNearbyPlants : HarvestPlantAbility
    {
        [Header("Freeze Settings")] 
        [SerializeField] private int freezeDuration = 3;

        public override void OnHarvest(PlantAbilityContext context)
        {
            List<PlantBase> affectedPlants = context.GardenManager.GetSurroundingPlants(
                context.OriginTile.GardenPosition.x,
                context.OriginTile.GardenPosition.y,
                effectRadius
            );

            foreach (PlantBase plant in affectedPlants)
            {
                if (!plant.HasBehavior<FreezableBehaviorInstance>()) continue;
                
                plant.SendSignal(PlantSignal.Freeze, new EffectDuration(freezeDuration));
            }
        }
    }
}
