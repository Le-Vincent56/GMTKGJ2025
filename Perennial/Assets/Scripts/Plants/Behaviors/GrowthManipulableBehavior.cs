using UnityEngine;

namespace Perennial.Plants.Behaviors
{
    [CreateAssetMenu(fileName = "Growth Manipulable Behavior", menuName="Plants/Behaviors/Growth Manipulable")]
    public class GrowthManipulableBehavior : PlantBehavior
    {
        [Header("Growth Settings")] 
        [SerializeField] private float baseGrowthRate = 1f;

        public override PlantBehaviorInstance CreateInstance(PlantBase plant)
        {
            return new GrowthManipulableBehaviorInstance(plant, this, baseGrowthRate);
        }
    }

    public class GrowthManipulableBehaviorInstance : PlantBehaviorInstance
    {
        private float _baseGrowthRate;
        private float _currentGrowthRate;

        public GrowthManipulableBehaviorInstance(PlantBase owner, PlantBehavior definition, float baseGrowthRate)
            : base(owner, definition)
        {
            _baseGrowthRate = baseGrowthRate;
        }

        public override bool HandleSignal(PlantSignal signalType, object data)
        {
            throw new System.NotImplementedException();
        }
    }
}