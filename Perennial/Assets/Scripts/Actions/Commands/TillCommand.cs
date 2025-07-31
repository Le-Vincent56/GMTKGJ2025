using System.Threading.Tasks;
using UnityEngine;

namespace Perennial.Actions.Commands
{
    public class TillCommand : BaseCommand
    {
        /// <summary>
        /// Executes the Till action
        /// </summary>
        public override async Task Execute()
        {
            Debug.Log("Tilling ground");
            await Awaitable.WaitForSecondsAsync(3f); //TODO ADD LOGIC
            Debug.Log("Finished tilling");
        }
    }
}
