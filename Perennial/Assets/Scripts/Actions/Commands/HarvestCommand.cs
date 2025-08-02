using System.Threading.Tasks;
using Perennial.Plants;


namespace Perennial.Actions.Commands
{
    public class HarvestCommand : BaseCommand
    {
        //hold reference to the plant as PlantStorageController needs reference to calculate seeds
        //could be a better queue way or another event to do this but this was easiest
        public Plant HarvestedPlant { get; }
        public HarvestCommand(HarvestArgs input) : base(input)
        {
            HarvestedPlant = input.Plant;
        }
        
        /// <summary>
        /// Executes the Harvest  action
        /// </summary>
        public override async Task Execute()
        {
            HarvestedPlant.Harvest();
            gardenManager.RemovePlantFromTile(Tile);
            
            //seed increase is taken care of in PlantStorageController
        }
    }
}
