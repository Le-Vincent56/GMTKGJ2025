using Perennial.Plants.Data;
using UnityEngine;

namespace Perennial.Plants.Mutations
{
    public class MutationWeightSettings : ScriptableObject, IMutationWeightCalculator
    {
        [Header("Base Weights")]
        [SerializeField] private float cardinalBaseWeight = 100f;
        [SerializeField] private float diagonalBaseWeight = 50f;
        
        [Header("Age Multiplier")]
        [SerializeField]
        private float ageMultiplierPerMonth = 0.1f;
        
        [Header("Yield Multiplier")]
        [SerializeField]
        private float yieldDivisor = 100f;
        
        [Header("Debug")]
        [SerializeField] private bool enableDebugLogging = false;
        
        public MutationCandidate CalculateWeight(Plant harvestedPlant, Plant neighborPlant, 
            PlantDefinition resultingPlant, ProximityType proximity)
        {
            // Get base weight from proximity
            float baseWeight = GetBaseWeight(proximity);
            
            // Calculate age multiplier
            float monthsAlive = neighborPlant.Lifetime.CurrentLifetime.Value;
            float ageMultiplier = CalculateAgeMultiplier(monthsAlive);
            
            // Calculate projected food yield
            Food projectedFood = neighborPlant.Rewards.CalculateFood();
            float yieldMultiplier = CalculateYieldMultiplier((float)projectedFood);
            
            // Calculate final weight
            float finalWeight = baseWeight * ageMultiplier * yieldMultiplier;
            
            if (enableDebugLogging)
            {
                Debug.Log($"Mutation Weight Calculation - {neighborPlant.Name} -> {resultingPlant.Name}:\n" +
                         $"  Base Weight ({proximity}): {baseWeight}\n" +
                         $"  Age ({monthsAlive:F1} months): x{ageMultiplier:F2}\n" +
                         $"  Yield ({(float)projectedFood:F1} food): x{yieldMultiplier:F2}\n" +
                         $"  Final Weight: {finalWeight:F2}");
            }
            
            return new MutationCandidate
            {
                NeighborPlant = neighborPlant.Definition,
                ResultingPlant = resultingPlant,
                Weight = finalWeight,
                PercentChance = 0f, // Will be calculated during normalization
                ProximityType = proximity,
                AgeMultiplier = ageMultiplier,
                YieldMultiplier = yieldMultiplier,
                ProjectedFood = projectedFood
            };
        }
        
        public float GetBaseWeight(ProximityType proximity)
        {
            return proximity == ProximityType.Cardinal ? cardinalBaseWeight : diagonalBaseWeight;
        }
        
        public float CalculateAgeMultiplier(float monthsAlive)
        {
            return 1f + (monthsAlive * ageMultiplierPerMonth);
        }
        
        public float CalculateYieldMultiplier(float projectedFood)
        {
            return 1f + (projectedFood / yieldDivisor);
        }
        
        private void OnValidate()
        {
            // Ensure values are reasonable
            cardinalBaseWeight = Mathf.Max(0.1f, cardinalBaseWeight);
            diagonalBaseWeight = Mathf.Max(0.1f, diagonalBaseWeight);
            ageMultiplierPerMonth = Mathf.Clamp(ageMultiplierPerMonth, 0f, 1f);
            yieldDivisor = Mathf.Max(1f, yieldDivisor);
        }
    }
}