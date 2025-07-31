using System.Threading.Tasks;
using UnityEngine;

namespace Perennial.Actions.Commands
{
    public class PlantCommand : BaseCommand
    {
        /// <summary>
        /// Executes the Plant action
        /// </summary>
        public override  async Task Execute()
        {
            Debug.Log("Planting a plant");
            await Awaitable.WaitForSecondsAsync(3f); //TODO temp line
            Debug.Log("Finished planting");
        }
    }
}
