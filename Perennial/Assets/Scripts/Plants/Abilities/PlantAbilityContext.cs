using Perennial.Garden;

namespace Perennial.Plants.Abilities
{
    public class PlantAbilityContext
    {
        public Plant Plant { get; }
        public Tile OriginTile { get; }
        public GardenManager GardenManager { get; }

        public PlantAbilityContext(Plant plant, Tile originTile, GardenManager gardenManager)
        {
            Plant = plant;
            OriginTile = originTile;
            GardenManager = gardenManager;
        }
    }
}