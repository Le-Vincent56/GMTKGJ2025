using System.Threading.Tasks;
using Perennial.Garden;
using UnityEngine;

namespace Perennial.Actions.Commands
{
    public class HarvestCommand : BaseCommand
    {
        protected HarvestCommand(GardenManager gardenManager, Tile tile) : base(gardenManager, tile)
        {
            
        }
        
        /// <summary>
        /// Executes the Harvest  action
        /// </summary>
        public override async Task Execute()
        {
            Debug.Log("Harvesting a plant");
            await Awaitable.WaitForSecondsAsync(3f); //TODO ADD LOGIC
            Debug.Log("Finished harvesting");
        }
    }
}
