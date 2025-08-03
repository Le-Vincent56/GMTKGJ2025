using Perennial.Plants.Data;

namespace Perennial.Plants.Mutations
{
    public enum ProximityType
    {
        Cardinal, 
        Diagonal
    }
    
    public struct MutationCandidate
    {
        public PlantDefinition NeighborPlant { get; set; }
        public PlantDefinition ResultingPlant { get; set; }
        public float Weight { get; set; }
        public float PercentChance { get; set; }
        public ProximityType ProximityType { get; set; }
        
        // Debug info
        public float AgeMultiplier { get; set; }
        public float YieldMultiplier { get; set; }
        public Food ProjectedFood { get; set; }
    }
}
