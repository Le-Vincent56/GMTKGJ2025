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
        private readonly List<PlantBehaviorInstance> _behaviors = new List<PlantBehaviorInstance>();
        private readonly Tile _currentTile;
        private readonly GardenManager _gardenManager;

        public SerializableGuid ID { get; private set; }
        public string Name => _definition.Name;
        public Lifetime CurrentLifetime { get; private set; }
        public Lifetime TotalLifetime { get; private set; }
        public bool SkipGrowth { get; set; }
        public bool SkipPassive { get; set; }
        public List<PlantBehaviorInstance> Behaviors => _behaviors;
        public PlantStats Stats { get; }

        public Plant(PlantDefinition definition, Tile currentTile, GardenManager gardenManager)
        {
            ID = SerializableGuid.NewGuid();
            _definition = definition;
            _currentTile = currentTile;
            _gardenManager = gardenManager;
            _abilities = definition.Abilities;
            Stats = new PlantStats(definition.BaseStats);
            CurrentLifetime = (Lifetime)0;
            TotalLifetime = (Lifetime)definition.Lifetime;
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

            // If not skipping growth, grow the plant
            if (!SkipGrowth) CurrentLifetime += Stats.GrowthRate;
            
            // Exit early if skipping the passive ability
            if (SkipPassive) return;

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

        public void Harvest()
        {
            // Get the context for the plant ability
            PlantAbilityContext context = CreateAbilityContext();

            // Let behaviors process
            foreach (PlantBehaviorInstance behavior in _behaviors)
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

        private PlantAbilityContext CreateAbilityContext()
        {
            // TODO: Get the current turn from somewhere
            int currentTurn = 0;
            return new PlantAbilityContext(this, _currentTile, _gardenManager, currentTurn);
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
