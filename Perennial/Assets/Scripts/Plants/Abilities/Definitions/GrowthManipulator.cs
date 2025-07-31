using System.Collections.Generic;
using Perennial.Plants.Behaviors;
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
            List<PlantBase> affectedPlants = context.Garden.GetSurroundingPlants(
                context.OriginTile.GardenPosition.x,
                context.OriginTile.GardenPosition.y,
                effectRadius
            );

            foreach (PlantBase plant in affectedPlants)
            {
                if (plant.HasBehavior<GrowthManipulableBehaviorInstance>()) continue;
                
                plant.SendSignal(PlantSignal.Grow, growthMultiplier);
            }
        }
    }
}