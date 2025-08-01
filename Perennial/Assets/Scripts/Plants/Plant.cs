using System;
using System.Collections.Generic;
using System.Linq;
using Perennial.Core.Extensions;
using Perennial.Garden;
using Perennial.Plants.Abilities;
using Perennial.Plants.Behaviors;
using Perennial.Plants.Stats;
using Perennial.Seasons;

namespace Perennial.Plants
{
    public class Plant : IPlant
    {
        private readonly PlantDefinition _definition;
        private readonly PlantAbility[] _abilities;
        private readonly GardenManager _gardenManager;

        public SerializableGuid ID { get; private set; }
        public string Name => _definition.Name;
        
        public bool SkipGrowth { get; set; }
        public bool SkipPassive { get; set; }
        public Tile Tile { get; }
        public List<PlantBehaviorInstance> Behaviors { get; } = new List<PlantBehaviorInstance>();
        public PlantStats Stats { get; }
        public PlantLifetime Lifetime { get; }
        public PlantRewards Rewards { get; }
        public bool MarkedForRemoval { get; private set; }

        public Plant(PlantDefinition definition, Tile currentTile, GardenManager gardenManager)
        {
            ID = SerializableGuid.NewGuid();
            _definition = definition;
            _gardenManager = gardenManager;
            _abilities = definition.Abilities;
            Tile = currentTile;
            Stats = new PlantStats(definition.BaseStats);
            Lifetime =  new PlantLifetime(definition.GrowTime, definition.HarvestTime);
            Rewards = new PlantRewards(this, definition.FoodMultiplier, definition.FoodConstant);
            MarkedForRemoval = false;
        }

        /// <summary>
        /// Run plant logic for placement
        /// </summary>
        public void Place()
        {
            // Trigger placement abilities
            PlantAbilityContext context = CreateAbilityContext();
            TriggerPlaceAbilities(context);
        }

        /// <summary>
        /// Run plant logic for the turn
        /// </summary>
        public void Upkeep()
        {
            // Reset Skip Growth (should be overridden by behaviors)
            SkipGrowth = false;
            SkipPassive = false;

            PlantAbilityContext context = CreateAbilityContext();
            ProcessPassiveBehaviors(context);

            // If not skipping growth, grow the plant
            if (SkipGrowth) return;
            
            // Increase the lifetime
            Lifetime.Grow(Stats.GrowthRate);
        }

        /// <summary>
        /// Run plant logic for harvesting the plant
        /// </summary>
        public void Harvest()
        {
            PlantAbilityContext context = CreateAbilityContext();
            ProcessHarvestBehaviors(context);
            TriggerHarvestAbilities(context);
            CancelPassiveAbilities(context);

            // TODO: Remove the plant from the garden
            // TODO: Send rewards to the player
        }

        /// <summary>
        /// Have teh season affect the plant
        /// </summary>
        public void ApplySeasonalEffects(Season currentSeason)
        {
            void CheckSeason(Action notEqual, Action equal)
            {
                // Iterate through each incompatible season
                foreach (Season season in _definition.IncompatibleSeasons)
                {
                    // Skip if the season is not the current season
                    if (season != currentSeason) notEqual();
                    else equal();
                }
            }
            
            // Check for incompatible seasons
            CheckSeason(
                () => { }, 
                () => MarkedForRemoval = true
            );
            
            // Check for bonus seasons
            CheckSeason(
                () => Rewards.BonusActive = false, 
                () => Rewards.BonusActive = true
            );
        }

        /// <summary>
        /// Check if plants should be disposed due to lifetime
        /// </summary>
        public void CheckExpiration()
        {
            // Exit if the lifetime hasn't expired yet
            if(Lifetime.IsAlive()) return;

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

            // Get the context for the plant ability
            PlantAbilityContext context = CreateAbilityContext();
            TriggerPassiveAbilities(context);
        }

        /// <summary>
        /// Trigger all Place abilities
        /// </summary>
        private void TriggerPlaceAbilities(PlantAbilityContext context)
        {
            // Exit if the plant is not fully grown
            if (!Lifetime.FullyGrown) return;
            
            // Iterate through each ability
            foreach (PlantAbility ability in _abilities)
            {
                // Skip if not a passive plant ability
                if(ability is not PlacePlantAbility placeAbility) continue;

                // Activate the place ability
                placeAbility.OnPlace(context);
            }
        }

        /// <summary>
        /// Process Passive behaviors
        /// </summary>
        private void ProcessPassiveBehaviors(PlantAbilityContext context)
        {
            // Iterate through each behavior
            foreach (PlantBehaviorInstance behavior in Behaviors)
            {
                // Trigger its passive logic
                behavior.OnTick(context);
            }
        }

        /// <summary>
        /// Process Harvest behaviors
        /// </summary>
        private void ProcessHarvestBehaviors(PlantAbilityContext context)
        {
            // Iterate through each behavior
            foreach (PlantBehaviorInstance behavior in Behaviors)
            {
                // Trigger its harvest logic
                behavior.OnHarvest(context);
            }
        }

        /// <summary>
        /// Trigger all Harvest abilities
        /// </summary>
        private void TriggerHarvestAbilities(PlantAbilityContext context)
        {
            // Iterate through each ability
            foreach (PlantAbility ability in _abilities)
            {
                // Skip if not a harvest ability
                if (ability is not HarvestPlantAbility harvestAbility) continue;
                
                // Skip if the harvest ability cannot execute
                if (!harvestAbility.CanExecute(context)) continue;
                
                // Tick the harvest ability
                harvestAbility.OnHarvest(context);
            }
        }
        
        /// <summary>
        /// Cancel all Passive abilities
        /// </summary>
        private void CancelPassiveAbilities(PlantAbilityContext context)
        {
            // Iterate through each ability
            foreach (PlantAbility ability in _abilities)
            {
                // Skip if not a passive plant ability
                if(ability is not PassivePlantAbility passiveAbility) continue;

                // Cancel the passive ability
                passiveAbility.Cancel(context);
            }
        }

        /// <summary>
        /// Trigger all Passive abilities
        /// </summary>
        private void TriggerPassiveAbilities(PlantAbilityContext context)
        {
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

        /// <summary>
        /// Allocate the context necessary for Plant abilities
        /// </summary>
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
