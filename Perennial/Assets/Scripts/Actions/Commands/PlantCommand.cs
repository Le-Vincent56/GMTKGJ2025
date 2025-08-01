using System.Threading.Tasks;
using Perennial.Garden;
using UnityEngine;

namespace Perennial.Actions.Commands
{
    public class PlantCommand : BaseCommand
    {
        protected PlantCommand(GardenManager gardenManager) : base(gardenManager)
        {

        }
        
        /// <summary>
        /// Executes the Plant action
        /// </summary>
        public override  async Task Execute()
        {
            Debug.Log("Planting a plant");
            await Awaitable.WaitForSecondsAsync(3f);  //TODO ADD LOGIC
            Debug.Log("Finished planting");
        }
    }
}
