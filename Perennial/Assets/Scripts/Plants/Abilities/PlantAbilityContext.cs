namespace Perennial.Plants.Abilities
{
    public class PlantAbilityContext
    {
        public PlantBase Plant { get; }
        public Tile OriginTile { get; }
        public Garden Garden { get; }
        public int CurrentTurn { get; }

        public PlantAbilityContext(PlantBase plant, Tile originTile, Garden garden, int currentTurn)
        {
            Plant = plant;
            OriginTile = originTile;
            Garden = garden;
            CurrentTurn = currentTurn;
        }
    }
}