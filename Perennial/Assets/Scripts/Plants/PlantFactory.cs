using Perennial.Plants.Behaviors;

namespace Perennial.Plants
{
    public static class PlantFactory
    {
        public static PlantBase CreatePlant(PlantDefinition definition, Tile tile = null, Garden garden = null)
        {
            // Create a new plant
            Plant plant = new Plant(definition, tile, garden);

            foreach (PlantBehavior behaviorDefinition in definition.Behaviors)
            {
                if (behaviorDefinition == null) continue;
                
                // Create the behavior instance and add to the plant
                PlantBehaviorInstance instance = behaviorDefinition.CreateInstance(plant);
                plant.Behaviors.Add(instance);
            }

            return plant;
        }
    }
}