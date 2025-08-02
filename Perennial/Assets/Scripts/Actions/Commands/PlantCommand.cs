using System.Threading.Tasks;
using Perennial.Garden;
using Perennial.Plants;
using UnityEngine;

namespace Perennial.Actions.Commands
{
    public class PlantCommand : BaseCommand
    {
        private readonly PlantDefinition _plant;
        public PlantCommand(PlantArgs input) : base(input)
        {
            
            _plant = input.PlantDefinition;
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
