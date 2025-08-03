using System;
using System.Collections.Generic;
using System.Linq;
using Perennial.Core.Debugging;
using Perennial.Core.Extensions;
using UnityEngine;
using LogType = Perennial.Core.Debugging.LogType;

namespace Perennial.Plants.Mutations
{
    [CreateAssetMenu(fileName = "Mutation Table", menuName = "Plants/Mutation Table")]
    public class MutationTable : ScriptableObject
    {
        [Header("Mutation Rules")]
        [SerializeField] private List<MutationRule> mutationRules = new List<MutationRule>();
        
        // Runtime lookup cache for performance
        private Dictionary<(SerializableGuid, SerializableGuid), PlantDefinition> _lookupCache = new Dictionary<(SerializableGuid, SerializableGuid), PlantDefinition>();
        
        /// <summary>
        /// Get the mutation result for two parent plants
        /// </summary>
        public PlantDefinition GetMutationResult(PlantDefinition parent1, PlantDefinition parent2)
        {
            // Exit if the cache doesn't exist
            if (_lookupCache == null) return null;
            
            // Exit case - A plant cannot mutate with itself
            if (parent1 == parent2) return null;
            
            // Build cache if needed
            if (_lookupCache == null) BuildLookupCache();
            
            // Create ordered key for lookup (smaller ID first for consistency)
            (SerializableGuid, SerializableGuid) key = CreateOrderedKey(parent1.ID, parent2.ID);
            
            // Return cached result
            return _lookupCache.GetValueOrDefault(key);
        }
        
        /// <summary>
        /// Check if two plants can mutate together
        /// </summary>
        public bool CanMutate(PlantDefinition parent1, PlantDefinition parent2)
        {
            return GetMutationResult(parent1, parent2);
        }
        
        /// <summary>
        /// Build the lookup cache for fast runtime access
        /// </summary>
        private void BuildLookupCache()
        {
            _lookupCache = new Dictionary<(SerializableGuid, SerializableGuid), PlantDefinition>();
            
            foreach (MutationRule rule in mutationRules)
            {
                // Skip if not every part of the rule is filled out
                if (!rule.Parent1 || !rule.Parent2 || !rule.Result)
                    continue;

                // Create an ordered key for consistent lookup
                (SerializableGuid, SerializableGuid) key = CreateOrderedKey(rule.Parent1.ID, rule.Parent2.ID);
                _lookupCache[key] = rule.Result;
            }
        }
        
        /// <summary>
        /// Create an ordered key for consistent lookup
        /// </summary>
        private (SerializableGuid, SerializableGuid) CreateOrderedKey(SerializableGuid id1, SerializableGuid id2)
        {
            // Use string comparison to ensure consistent ordering
            return string.Compare(id1.ToString(), id2.ToString(), StringComparison.Ordinal) < 0 
                ? (id1, id2) 
                : (id2, id1);
        }
        
        /// <summary>
        /// Validate the mutation table for completeness and duplicates
        /// </summary>
        public void ValidateTable(List<PlantDefinition> allPlants)
        {
            // Check for duplicates
            IEnumerable<MutationRule> duplicates = mutationRules
                .GroupBy(r => CreateOrderedKey(r.Parent1?.ID ?? SerializableGuid.Empty, 
                                               r.Parent2?.ID ?? SerializableGuid.Empty))
                .Where(g => g.Count() > 1)
                .Select(g => g.First());
            
            foreach (MutationRule dup in duplicates)
            {
                Debugger.Log($"Duplicate mutation rule found: {dup.Parent1?.Name} + {dup.Parent2?.Name}", LogType.Warning);
            }
            
            // Check for missing combinations
            int expectedCombinations = (allPlants.Count * (allPlants.Count - 1)) / 2;
            int actualCombinations = mutationRules.Count(r => r.Parent1 && r.Parent2);
            
            // Check if the actual combinations are less than the expected amount
            if (actualCombinations < expectedCombinations)
            {
                Debugger.Log($"Mutation table incomplete: {actualCombinations}/{expectedCombinations} combinations defined", 
                    LogType.Warning);
            }
        }
    }
}
