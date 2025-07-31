using System.Collections.Generic;
using System.Linq;
using Perennial.Plants.Abilities;
using Perennial.Plants.Behaviors;

namespace Perennial.Plants
{
    public class PlantBase : IPlant
    {
        private PlantDefinition _definition;
        private PlantAbility[] _abilities;
        private readonly List<PlantBehaviorInstance> _behaviors = new List<PlantBehaviorInstance>();
        private Tile _currentTile;
        private Garden _garden;

        public string Name => _definition.Name;
        public int CurrentLifetime { get; protected set; }
        public int TotalLifetime => _definition.Lifetime;
        public bool SkipGrowth { get; set; }
        public bool SkipPassive { get; set; }
        public List<PlantBehaviorInstance> Behaviors => _behaviors;

        protected PlantBase(PlantDefinition definition, Tile currentTile, Garden garden)
        {
            this._definition = definition;
            this._currentTile = currentTile;
            this._garden = garden;
            _abilities = definition.Abilities;
            CurrentLifetime = 0;
        }

        public void Tick()
        {
            // Reset Skip Growth (should be overridden by behaviors)
            SkipGrowth = false;
            SkipPassive = false;

            // Get the context for the ability
            PlantAbilityContext context = CreateAbilityContext();
            
            // Let behaviors process
            foreach (PlantBehaviorInstance behavior in _behaviors)
            {
                behavior.OnTick(context);
            }

            // If not skipping growth, 
            if (!SkipGrowth) CurrentLifetime++;
            if (SkipPassive) return;

            foreach (PlantAbility ability in _abilities)
            {
                // Skip if ot a passive ability
                if (ability is not PassivePlantAbility passiveAbility) continue;
                
                // Skip if the passive ability cannot execute
                if (!passiveAbility.CanExecute(context)) continue;
                
                // Tick the passive ability
                passiveAbility.OnTick(context);
            }
        }

        public virtual void Harvest()
        {
            // Get the context for the plant ability
            PlantAbilityContext context = CreateAbilityContext();

            // Let behaviors process
            foreach (PlantBehaviorInstance behavior in _behaviors)
            {
                behavior.OnHarvest(context);
            }
            
            // Trigger abilities
            foreach (PlantAbility ability in _abilities)
            {
                // Skip if not a harvest ability
                if (ability is not HarvestPlantAbility harvestAbility) continue;
                
                // Skip if the harvest ability cannot execute
                if (!harvestAbility.CanExecute(context)) continue;
                
                // Tick the harvest ability
                harvestAbility.OnHarvest(context);
            }
            
            // TODO: Remove the plant from the garden
        }

        private PlantAbilityContext CreateAbilityContext()
        {
            // TODO: Get the current turn from somewhere
            int currentTurn = 0;
            return new PlantAbilityContext(this, _currentTile, _garden, currentTurn);
        }
        
        /// <summary>
        /// Send a signal to this plant's behaviors
        /// </summary>
        public bool SendSignal(PlantSignal signalType, object data)
        {
            bool handled = false;

            foreach (PlantBehaviorInstance behavior in _behaviors)
            {
                if (behavior.HandleSignal(signalType, data)) handled = true;
            }

            return handled;
        }
        
        /// <summary>
        /// Check if this plant has a specific behavior type
        /// </summary>
        public bool HasBehavior<T>() where T : PlantBehaviorInstance => _behaviors.Any(b => b is T);

        /// <summary>
        /// Get a specific behavior instance
        /// </summary>
        public T GetBehavior<T>() where T : PlantBehaviorInstance => _behaviors.FirstOrDefault(b => b is T) as T;
    }
}
