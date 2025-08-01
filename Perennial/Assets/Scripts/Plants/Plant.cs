using System.Collections.Generic;
using System.Linq;
using Perennial.Core.Extensions;
using Perennial.Garden;
using Perennial.Plants.Abilities;
using Perennial.Plants.Behaviors;
using Perennial.Plants.Data;
using Perennial.Plants.Stats;

namespace Perennial.Plants
{
    public class Plant : IPlant
    {
        private readonly PlantDefinition _definition;
        private readonly PlantAbility[] _abilities;
        private readonly GardenManager _gardenManager;

        public SerializableGuid ID { get; private set; }
        public string Name => _definition.Name;
        public Lifetime CurrentLifetime { get; private set; }
        public Lifetime TotalLifetime { get; private set; }
        public bool SkipGrowth { get; set; }
        public bool SkipPassive { get; set; }
        public Tile Tile { get; }
        public List<PlantBehaviorInstance> Behaviors { get; } = new List<PlantBehaviorInstance>();
        public PlantStats Stats { get; }
        public bool MarkedForRemoval { get; private set; }

        public Plant(PlantDefinition definition, Tile currentTile, GardenManager gardenManager)
        {
            ID = SerializableGuid.NewGuid();
            _definition = definition;
            _gardenManager = gardenManager;
            _abilities = definition.Abilities;
            Tile = currentTile;
            Stats = new PlantStats(definition.BaseStats);
            CurrentLifetime = (Lifetime)0;
            TotalLifetime = (Lifetime)definition.Lifetime;
            MarkedForRemoval = false;
        }

        /// <summary>
        /// Run plant logic for the turn
        /// </summary>
        public void Upkeep()
        {
            // Reset Skip Growth (should be overridden by behaviors)
            SkipGrowth = false;
            SkipPassive = false;

            // Get the context for the ability
            PlantAbilityContext context = CreateAbilityContext();
            
            // Let behaviors process
            foreach (PlantBehaviorInstance behavior in Behaviors)
            {
                behavior.OnTick(context);
            }

            // If not skipping growth, grow the plant
            if (!SkipGrowth) CurrentLifetime += Stats.GrowthRate;
        }

        /// <summary>
        /// Run plant logic for harvesting the plant
        /// </summary>
        public void Harvest()
        {
            // Get the context for the plant ability
            PlantAbilityContext context = CreateAbilityContext();

            // Let behaviors process
            foreach (PlantBehaviorInstance behavior in Behaviors)
            {
                behavior.OnHarvest(context);
            }
            
            // Trigger harvest abilities
            foreach (PlantAbility ability in _abilities)
            {
                // Skip if not a harvest ability
                if (ability is not HarvestPlantAbility harvestAbility) continue;
                
                // Skip if the harvest ability cannot execute
                if (!harvestAbility.CanExecute(context)) continue;
                
                // Tick the harvest ability
                harvestAbility.OnHarvest(context);
            }

            // Cancel passive abilities
            foreach (PlantAbility ability in _abilities)
            {
                // Skip if not a passive plant ability
                if(ability is not PassivePlantAbility passiveAbility) continue;

                // Cancel the passive ability
                passiveAbility.Cancel(context);
            }
            
            // TODO: Remove the plant from the garden
        }

        /// <summary>
        /// Check if plants should be disposed
        /// </summary>
        public void Dispose()
        {
            // Exit if the lifetime hasn't expired yet
            if (CurrentLifetime < TotalLifetime) return;

            // Mark the plant for removal
            MarkedForRemoval = true;
        }

        /// <summary>
        /// Run end of turn plant logic
        /// </summary>
        public void EndStep()
        {
            // Exit early if skipping the passive ability
            if (SkipPassive) return;

            TriggerPassiveTicks();
        }

        private void TriggerPassiveTicks()
        {
            // Get the context for the plant ability
            PlantAbilityContext context = CreateAbilityContext();

            // Iterate through each ability
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

        private PlantAbilityContext CreateAbilityContext() => new PlantAbilityContext(this, Tile, _gardenManager);
        
        /// <summary>
        /// Send a signal to this plant's behaviors
        /// </summary>
        public bool SendSignal(PlantSignal signalType, object data)
        {
            bool handled = false;

            foreach (PlantBehaviorInstance behavior in Behaviors)
            {
                if (behavior.HandleSignal(signalType, data)) handled = true;
            }

            return handled;
        }
        
        /// <summary>
        /// Check if this plant has a specific behavior type
        /// </summary>
        public bool HasBehavior<T>() where T : PlantBehaviorInstance => Behaviors.Any(b => b is T);

        /// <summary>
        /// Get a specific behavior instance
        /// </summary>
        public T GetBehavior<T>() where T : PlantBehaviorInstance => Behaviors.FirstOrDefault(b => b is T) as T;
    }
}
