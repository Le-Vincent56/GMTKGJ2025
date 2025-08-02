using System.Threading.Tasks;
using Perennial.Plants;


namespace Perennial.Actions.Commands
{
    public class HarvestCommand : BaseCommand
    {
        public HarvestCommand(HarvestArgs input) : base(input)
        {
            
        }
        
        /// <summary>
        /// Executes the Harvest  action
        /// </summary>
        public override async Task Execute()
        {
            Tile.Plant.Harvest();
            gardenManager.RemovePlantFromTile(Tile);
            //seed increase is taken care of in PlantStorageController
            
            
        }
    }
}
