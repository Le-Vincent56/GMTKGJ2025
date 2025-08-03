using Perennial.Plants.Data;
using UnityEngine;

namespace Perennial.Plants.Mutations
{
    [System.Serializable]
    public class DefaultMutationWeightCalculator : IMutationWeightCalculator
    {
        [Header("Base Weights")] 
        [SerializeField] private float cardinalBaseWeight = 100f;
        [SerializeField] private float diagonalBaseWeight = 50f;
            
        [Header("Multiplier Settings")]
        [SerializeField] private float ageMultiplierPerMonth = 0.1f;
        [SerializeField] private float yieldDivisor = 100f;

        public MutationCandidate CalculateWeight(Plant harvestedPlant, Plant neighborPlant, PlantDefinition resultingPlant,
            ProximityType proximityType)
        {
            // Get the base weight from proximity
            float baseWeight = GetBaseWeight(proximityType);
                
            // Calculate the age multiplier
            float monthsAlive = neighborPlant.Lifetime.CurrentLifetime.Value;
            float ageMultiplier = CalculateAgeMultiplier(monthsAlive);
                
            // Calculate the projected food yield
            Food projectedFood = neighborPlant.Rewards.CalculateFood();
            float yieldMultiplier = CalculateYieldMultiplier((float)projectedFood);
                
            // Calculate final weight
            float finalWeight = baseWeight * ageMultiplier * yieldMultiplier;

            return new MutationCandidate()
            {
                NeighborPlant = neighborPlant.Definition,
                ResultingPlant = resultingPlant,
                Weight = finalWeight,
                PercentChance = 0f, // Will be calculated during normalization
                ProximityType = proximityType,
                AgeMultiplier = ageMultiplierPerMonth,
                YieldMultiplier = yieldMultiplier,
                ProjectedFood = projectedFood,
            };
        }

        public float GetBaseWeight(ProximityType proximityType)
        {
            return proximityType == ProximityType.Cardinal
                ? cardinalBaseWeight
                : diagonalBaseWeight;
        }

        public float CalculateAgeMultiplier(float monthsAlive)
        {
            return 1f + (monthsAlive * ageMultiplierPerMonth);
        }

        public float CalculateYieldMultiplier(float projectedFood)
        {
            return 1f + (projectedFood / yieldDivisor);
        }
    }
}