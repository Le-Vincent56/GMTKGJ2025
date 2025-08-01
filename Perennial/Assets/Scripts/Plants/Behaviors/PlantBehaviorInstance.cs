using Perennial.Plants.Abilities;

namespace Perennial.Plants.Behaviors
{
    public abstract class PlantBehaviorInstance
    {
        protected readonly Plant owner;
        protected readonly PlantBehavior definition;
        
        public Plant Owner => owner;
        public PlantBehavior Definition => definition;

        public PlantBehaviorInstance(Plant owner, PlantBehavior definition)
        {
            this.definition = definition;
            this.owner = owner;
        }
        
        /// <summary>
        /// Called when the plant passively ticks
        /// </summary>
        public virtual void OnTick(PlantAbilityContext context) { }
        
        /// <summary>
        /// Called when the plant is harvested
        /// </summary>
        public virtual void OnHarvest(PlantAbilityContext context) { }

        /// <summary>
        /// Handle any signal sent to this plant
        /// </summary>
        public abstract bool HandleSignal(PlantSignal signalType, object data);
    }

    public enum PlantSignal
    {
        Freeze,
        Grow
    }
}