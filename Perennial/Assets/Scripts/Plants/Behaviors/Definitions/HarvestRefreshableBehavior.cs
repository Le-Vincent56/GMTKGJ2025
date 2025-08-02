using UnityEngine;

namespace Perennial.Plants.Behaviors.Definitions
{
    [CreateAssetMenu(fileName = "Harvest Refreshable Behavior", menuName="Plants/Behaviors/Harvest Refreshable")]
    public class HarvestRefreshableBehavior : PlantBehavior
    {
        public override PlantBehaviorInstance CreateInstance(Plant plant)
        {
            return new HarvestFreshableBehaviorInstance(plant, this);
        }
    }
    
    public class HarvestFreshableBehaviorInstance : PlantBehaviorInstance
    {
        public HarvestFreshableBehaviorInstance(Plant owner, HarvestRefreshableBehavior definition)
            : base(owner, definition)
        { }
        
        public override bool HandleSignal(PlantSignal signalType, object data)
        {
            // Exit if the signal is incorrect
            if (signalType != PlantSignal.RefreshHarvest) return false;
            
            owner.Lifetime.ResetToHarvestStart();
            
            return true;
        }
    }
}