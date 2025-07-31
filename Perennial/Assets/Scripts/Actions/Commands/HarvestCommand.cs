using UnityEngine;

namespace Perennial.Actions.Commands
{
    public class HarvestCommand : BaseCommand
    {
        /// <summary>
        /// Executes the Harvest  action
        /// </summary>
        public override void Execute()
        {
            Debug.Log("Harvesting a plant");
        }
    }
}
