using System.Threading.Tasks;
using Perennial.Plants;
using Perennial.Plants.Data;
using UnityEngine;


namespace Perennial.Actions.Commands
{
    public class HarvestCommand : BaseCommand
    {
        //reference to the plant this exists as the way I set up PlantStorage to listen it happens to sometimes trigger after plant is cleared from tile
        public Plant HarvestedPlant { get; }
        public HarvestCommand(HarvestArgs input) : base(input)
        {
            HarvestedPlant = input.Tile.Plant;
        }
        
        /// <summary>
        /// Executes the Harvest  action
        /// </summary>
        public override async Task Execute()
        {
            HarvestedPlant.Harvest(out (Food Food, Seeds BaseSeeds, Seeds? MutationSeeds) rewards);
            gardenManager.RemovePlantFromTile(Tile);
            
            //seed increase is taken care of in PlantStorageController
            
        }
    }
}
