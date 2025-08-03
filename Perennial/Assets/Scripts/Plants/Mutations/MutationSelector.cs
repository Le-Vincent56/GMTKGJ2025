using System.Collections.Generic;
using System.Linq;
using Perennial.Garden;
using UnityEngine;

namespace Perennial.Plants.Mutations
{
    public class MutationSelector
    {
        private readonly IMutationWeightCalculator _weightCalculator;

        public MutationSelector(IMutationWeightCalculator weightCalculator)
        {
            _weightCalculator = weightCalculator;
        }

        /// <summary>
        /// Calculate the result of a Mutation and all candidates with their probabilities
        /// </summary>
        public MutationSelectionResult SelectMutation(Plant harvestedPlant, GardenManager gardenManager)
        {
            // Get all valid mutation candidates
            List<MutationCandidate> candidates = GetMutationCandidates(harvestedPlant, gardenManager);

            if (candidates.Count == 0)
            {
                return new MutationSelectionResult()
                {
                    SelectedMutation = null,
                    AllCandidates = new List<MutationCandidate>()
                };
            }
            
            // Normalize the weights of each candidate into percentages
            NormalizeWeights(candidates);
            
            // Select mutation using weighted random
            MutationCandidate? selected = SelectWeightRandom(candidates);

            return new MutationSelectionResult()
            {
                SelectedMutation = selected,
                AllCandidates = candidates
            };
        }

        /// <summary>
        /// Get all candidates for Mutation around a harvested plant
        /// </summary>
        private List<MutationCandidate> GetMutationCandidates(Plant harvestedPlant, GardenManager gardenManager)
        {
            List<MutationCandidate> candidates = new List<MutationCandidate>();
            
            // Get the tile position
            Vector2Int position = harvestedPlant.Tile.GardenPosition;
            
            // Check all 8 surrounding positions
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    // Skip the center check
                    if (dx == 0 && dy == 0) continue;

                    // Get the plant at the position
                    Plant neighbor = gardenManager.GetPlantAtPosition(position.x + dx, position.y + dy);
                    
                    // Skip if no plant or not harvestable
                    if (neighbor == null || !neighbor.Lifetime.FullyGrown) continue;
                    
                    // Check if a mutation exists
                    PlantDefinition mutationResult = MutationManager.Instance.GetMutationResult(
                        harvestedPlant.Definition, 
                        neighbor.Definition
                    );
                    
                    // Skip if a mutation doesn't exist
                    if(mutationResult == null) continue;
                    
                    // Determine the proximity type
                    ProximityType proximity = (dx == 0 || dy == 0)
                        ? ProximityType.Cardinal
                        : ProximityType.Diagonal;
                    
                    // Calculate the weight and add as a candidate
                    MutationCandidate candidate = _weightCalculator.CalculateWeight(
                        harvestedPlant,
                        neighbor,
                        mutationResult,
                        proximity
                    );
                    
                    candidates.Add(candidate);
                }
            }

            return candidates;
        }

        /// <summary>
        /// Normalize the weights of all candidates to store their percent chance data
        /// </summary>
        private void NormalizeWeights(List<MutationCandidate> candidates)
        {
            // Add up all the weights
            float totalWeight = candidates.Sum(c => c.Weight);
            
            // Exit if the total weight is less than or equal to 0
            if (totalWeight <= 0) return;
            
            // Iterate through each candidate
            for (int i = 0; i < candidates.Count; i++)
            {
                // Set the percent data
                MutationCandidate candidate = candidates[i];
                candidate.PercentChance = (candidate.Weight / totalWeight) * 100f;
                candidates[i] = candidate;
            }
        }

        /// <summary>
        /// Select a Mutation Candidate based on its weight and some randomness
        /// </summary>
        private MutationCandidate? SelectWeightRandom(List<MutationCandidate> candidates)
        {
            // Exit if there are no candidates
            if (candidates.Count == 0) return null;
            
            float random = Random.Range(0f, 100f);
            float cumulative = 0f;

            // Iterate through each candidate
            foreach (MutationCandidate candidate in candidates)
            {
                cumulative += candidate.PercentChance;
                if (random <= cumulative) return candidate;
            }
            
            // Fallback (shouldn't happen with proper normalization)
            return candidates[^1];
        }
    }
}