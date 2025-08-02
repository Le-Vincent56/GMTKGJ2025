using Perennial.Plants.Abilities;
using Perennial.Plants.Data;
using UnityEngine;

namespace Perennial.Plants.Behaviors
{
    [CreateAssetMenu(fileName = "Freezable Behavior", menuName="Plants/Behaviors/Freezable")]
    public class FreezableBehavior : PlantBehavior
    {
        [Header("Freeze Settings")] 
        [SerializeField] bool canBeFrozen = true;

        public override PlantBehaviorInstance CreateInstance(Plant plant)
        {
            return new FreezableBehaviorInstance(plant, this, canBeFrozen);
        }
    }
    
    public class FreezableBehaviorInstance : PlantBehaviorInstance
    {
        private EffectDuration _frozenTurnsRemaining;
        private readonly bool _canBeFrozen;
        
        public bool IsFrozen => _frozenTurnsRemaining > 0;

        public FreezableBehaviorInstance(Plant owner, FreezableBehavior definition, bool canBeFrozen)
            : base(owner, definition)
        {
            _canBeFrozen = canBeFrozen;
            _frozenTurnsRemaining = (EffectDuration)0;
        }

        public override void OnTick(PlantAbilityContext context)
        {
            // Exit if not frozen
            if (!IsFrozen) return;
            
            // Reduce the amount of turns remaining and skip growth and passive ticks
            _frozenTurnsRemaining--;
            owner.SkipGrowth = true;
            owner.SkipPassive = true;
        }
        
        public override bool HandleSignal(PlantSignal signalType, object data)
        {
            // Exit if not signaled to be frozen or the plant cannot be frozen
            if (signalType != PlantSignal.Freeze || !_canBeFrozen) return false;
            
            // Exit if the data passed is not a duration
            if (data is not EffectDuration duration) return false;
            
            // Set the amount of turns to be frozen for
            _frozenTurnsRemaining = duration;
            return true;
        }
    }
}