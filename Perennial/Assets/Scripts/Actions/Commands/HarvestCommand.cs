using System.Threading.Tasks;
using UnityEngine;

namespace Perennial.Actions.Commands
{
    public class HarvestCommand : BaseCommand
    {
        /// <summary>
        /// Executes the Harvest  action
        /// </summary>
        public override async Task Execute()
        {
            Debug.Log("Harvesting a plant");
            await Awaitable.WaitForSecondsAsync(3f); //TODO temp line
            Debug.Log("Finished harvesting");
        }
    }
}
