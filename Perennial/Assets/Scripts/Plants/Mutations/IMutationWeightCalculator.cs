namespace Perennial.Plants.Mutations
{
    public interface IMutationWeightCalculator
    {
        /// <summary>
        /// Calculate the weight for a mutation candidate based on
        /// proximity, age, and yield
        /// </summary>
        MutationCandidate CalculateWeight(Plant harvestedPlant, Plant neighborPlant,
            PlantDefinition resultingPlant, ProximityType proximityType);
        
        /// <summary>
        /// Calculate the base weight for the proximity type
        /// </summary>
        float GetBaseWeight(ProximityType proximityType);

        /// <summary>
        /// Calculate the age weight multiplier
        /// </summary>
        float CalculateAgeMultiplier(float monthsAlive);
        
        /// <summary>
        /// Calculate the yield weight multiplier
        /// </summary>
        /// <param name="monthsAlive"></param>
        /// <returns></returns>
        float CalculateYieldMultiplier(float monthsAlive);
    }
}