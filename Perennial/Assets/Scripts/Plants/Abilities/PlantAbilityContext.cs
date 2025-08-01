using Perennial.Garden;

namespace Perennial.Plants.Abilities
{
    public class PlantAbilityContext
    {
        public Plant Plant { get; }
        public Tile OriginTile { get; }
        public GardenManager GardenManager { get; }
        public int CurrentTurn { get; }

        public PlantAbilityContext(Plant plant, Tile originTile, GardenManager gardenManager, int currentTurn)
        {
            Plant = plant;
            OriginTile = originTile;
            GardenManager = gardenManager;
            CurrentTurn = currentTurn;
        }
    }
}